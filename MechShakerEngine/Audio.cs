using System.Timers;
using MechShakerEngine.Effects;
using MechShakerEngine.Events;
using MechShakerEngine.Interfaces;
using MechShakerEngine.Processors;
using MechShakerReader;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using Timer = System.Timers.Timer;

namespace MechShakerEngine;

internal class Audio : ISampleProvider
{
    public bool       Enabled      { get; private set; }
    public Volume     MasterVolume { get; private set; }
    public WaveFormat WaveFormat   { get; }
    public MMDevice?  OutputDevice { get; private set; }
    public int        Latency      { get; private set; }
    public uint       Clock        { get; private set; }
    public object     Lock         { get; } = new();

    public const int SampleRate = 44100;

    private readonly ISampleProvider _chainStart;

    private readonly EffectGroup          _weaponEffects;
    private readonly Lasers               _lasers;
    private readonly TAG                  _tag;
    private readonly Flamers              _flamers;
    private readonly MachineGuns          _machineGuns;
    private readonly AMS                  _ams;
    private readonly AutocannonsAndRifles _autocannonsAndRifles;
    private readonly PPCs                 _ppcs;
    private readonly Missiles             _missiles;

    private readonly EffectGroup    _impactEffects;
    private readonly Melee          _melee;
    private readonly Footsteps      _footsteps;
    private readonly LandingImpacts _landingImpacts;

    private readonly EffectGroup      _damageEffects;
    private readonly LaserDamage      _laserDamage;
    private readonly FlamerDamage     _flamerDamage;
    private readonly MachineGunDamage _machineGunDamage;
    private readonly ProjectileDamage _projectileDamage;
    private readonly MissileDamage    _missileDamage;
    private readonly MeleeDamage      _meleeDamage;
    private readonly ExplosionDamage  _explosionDamage;
    private readonly PartDestruction  _partDestruction;

    private readonly TorsoTwist _torsoTwist;
    private readonly JumpJets   _jumpJets;
    private readonly MASC       _masc;
    private readonly Dropship   _dropship;
    private readonly Powering   _powering;

    private readonly LowPassFilter   _lowPassFilter;
    private readonly HighPassFilter  _highPassFilter;
    private readonly ClippingCheck   _clippingCheck;
    private readonly ISampleProvider _chainEnd;

    private readonly List<ISampler> _samplers;

    private const    double EventTimeoutMs = 250;
    private readonly Timer  _timer         = new(EventTimeoutMs);


    public Audio()
    {
        MasterVolume = new Volume(0);
        WaveFormat   = WaveFormat.CreateIeeeFloatWaveFormat(SampleRate, 1);
        _chainStart  = new ChainStart(WaveFormat);

        // Weapon Effects Group
        _weaponEffects        = new EffectGroup(_chainStart);
        _lasers               = new Lasers(_weaponEffects, this);
        _tag                  = new TAG(_weaponEffects, this);
        _flamers              = new Flamers(_weaponEffects, this);
        _machineGuns          = new MachineGuns(_weaponEffects, this);
        _ams                  = new AMS(_weaponEffects, this);
        _autocannonsAndRifles = new AutocannonsAndRifles(_weaponEffects, this);
        _ppcs                 = new PPCs(_weaponEffects, this);
        _missiles             = new Missiles(_weaponEffects, this);
        _weaponEffects.AddEffect(_lasers);
        _weaponEffects.AddEffect(_tag);
        _weaponEffects.AddEffect(_flamers);
        _weaponEffects.AddEffect(_machineGuns);
        _weaponEffects.AddEffect(_ams);
        _weaponEffects.AddEffect(_autocannonsAndRifles);
        _weaponEffects.AddEffect(_ppcs);
        _weaponEffects.AddEffect(_missiles);


        // Impact Effects Group
        _impactEffects  = new EffectGroup(_weaponEffects);
        _melee          = new Melee(_impactEffects, this);
        _footsteps      = new Footsteps(_impactEffects, this);
        _landingImpacts = new LandingImpacts(_impactEffects, this);
        _impactEffects.AddEffect(_melee);
        _impactEffects.AddEffect(_footsteps);
        _impactEffects.AddEffect(_landingImpacts);


        // Damage effects group
        _damageEffects    = new EffectGroup(_impactEffects);
        _laserDamage      = new LaserDamage(_damageEffects, this);
        _flamerDamage     = new FlamerDamage(_damageEffects, this);
        _machineGunDamage = new MachineGunDamage(_damageEffects, this);
        _projectileDamage = new ProjectileDamage(_damageEffects, this);
        _missileDamage    = new MissileDamage(_damageEffects, this);
        _meleeDamage      = new MeleeDamage(_damageEffects, this);
        _explosionDamage  = new ExplosionDamage(_damageEffects, this);
        _partDestruction  = new PartDestruction(_damageEffects, this);
        _damageEffects.AddEffect(_laserDamage);
        _damageEffects.AddEffect(_flamerDamage);
        _damageEffects.AddEffect(_machineGunDamage);
        _damageEffects.AddEffect(_projectileDamage);
        _damageEffects.AddEffect(_missileDamage);
        _damageEffects.AddEffect(_meleeDamage);
        _damageEffects.AddEffect(_explosionDamage);
        _damageEffects.AddEffect(_partDestruction);


        _torsoTwist = new TorsoTwist(_damageEffects, this);
        _jumpJets   = new JumpJets(_torsoTwist, this);
        _masc       = new MASC(_jumpJets, this);
        _dropship   = new Dropship(_masc, this);
        _powering   = new Powering(_dropship, this);


        _lowPassFilter  = new LowPassFilter(_powering, 80, 2);
        _highPassFilter = new HighPassFilter(_lowPassFilter, 10, 2);
        _clippingCheck  = new ClippingCheck(_highPassFilter);
        _chainEnd       = _clippingCheck;

        _samplers = new List<ISampler>
        {
            _lasers,
            _tag,
            _flamers,
            _machineGuns,
            _ams,
            _autocannonsAndRifles,
            _ppcs,
            _missiles,
            _melee,
            _footsteps,
            _landingImpacts,
            _laserDamage,
            _flamerDamage,
            _machineGunDamage,
            _projectileDamage,
            _missileDamage,
            _meleeDamage,
            _explosionDamage,
            _partDestruction,
            _torsoTwist,
            _jumpJets,
            _masc,
            _dropship,
            _powering,
            _lowPassFilter,
            _highPassFilter,
            _clippingCheck
        };

        _timer.Elapsed   += OnEventDataTimeout;
        _timer.AutoReset =  false;
    }

    private void OnEventDataTimeout(object? sender, ElapsedEventArgs e)
    {
        Logging.At(this).Debug("Event data timed out, clearing sampler output...");
        foreach (var sampler in _samplers)
        {
            sampler.Clear();
        }
    }

    public void OnEventDataReceived(EventData eventData)
    {
        _timer.Stop();
        switch ((EventCode)eventData.EventCode)
        {
            case EventCode.BridgeClosed:
            case EventCode.ClearFX:
                foreach (var sampler in _samplers)
                {
                    sampler.Clear();
                }

                return;
            case EventCode.Trace:
                var traceEvt = new TraceEvent(eventData);
                if (traceEvt.IsMG)
                    _machineGuns.EventDataReceived(traceEvt);
                else if (traceEvt.IsFlamer)
                    _flamers.EventDataReceived(traceEvt);
                else if (traceEvt.IsTAG)
                    _tag.EventDataReceived(traceEvt);
                else if (traceEvt.IsAMS)
                    // Not sure if this will ever actually trigger? AMS seems to be it's own class type
                    break;
                else
                    _lasers.EventDataReceived(traceEvt);
                break;
            case EventCode.AMS:
                _ams.EventDataReceived(new AMSEvent(eventData));
                break;
            case EventCode.Projectile:
                var projectileEvt = new ProjectileEvent(eventData);
                if (projectileEvt.IsPPC)
                    _ppcs.EventDataReceived(projectileEvt);
                else
                    _autocannonsAndRifles.EventDataReceived(projectileEvt);
                break;
            case EventCode.Missiles:
                _missiles.EventDataReceived(new MissilesEvent(eventData));
                break;
            case EventCode.Melee:
                _melee.EventDataReceived(new MeleeEvent(eventData));
                break;
            case EventCode.Footstep:
                _footsteps.EventDataReceived(new FootstepEvent(eventData));
                break;
            case EventCode.Landed:
                _landingImpacts.EventDataReceived(new LandedEvent(eventData));
                break;
            case EventCode.TorsoTwist:
                _torsoTwist.EventDataReceived(new TorsoTwistEvent(eventData));
                break;
            case EventCode.JumpJets:
                _jumpJets.EventDataReceived(new JumpJetsEvent(eventData));
                break;
            case EventCode.Airborne:
                break;
            case EventCode.MASC:
                _masc.EventDataReceived(new MASCEvent(eventData));
                break;
            case EventCode.Dropship:
                _dropship.EventDataReceived(new DropshipEvent(eventData));
                break;
            case EventCode.Powering:
                _powering.EventDataReceived(new PoweringEvent(eventData));
                break;
            case EventCode.PartDestruction:
                _partDestruction.EventDataReceived(new PartDestructionEvent(eventData));
                break;
            case EventCode.Damaged:
                var damagedEvent = new DamagedEvent(eventData);
                switch (damagedEvent.Type)
                {
                    case DamagedEvent.DamageType.Trace:
                        if (damagedEvent.IsFlamer)
                            _flamerDamage.EventDataReceived(damagedEvent);
                        else if (damagedEvent.IsMG)
                            _machineGunDamage.EventDataReceived(damagedEvent);
                        else
                            _laserDamage.EventDataReceived(damagedEvent);
                        break;
                    case DamagedEvent.DamageType.Projectile:
                        _projectileDamage.EventDataReceived(damagedEvent);
                        break;
                    case DamagedEvent.DamageType.Missile:
                        _missileDamage.EventDataReceived(damagedEvent);
                        break;
                    case DamagedEvent.DamageType.Melee:
                        _meleeDamage.EventDataReceived(damagedEvent);
                        break;
                    case DamagedEvent.DamageType.Explosion:
                        _explosionDamage.EventDataReceived(damagedEvent);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                break;
            case EventCode.NULL:
            default:
                Logging.At(this).Warning("Invalid event code - {EventCode}", eventData.EventCode);
                break;
        }

        _timer.Start();
    }

    public void Start(string? deviceName, int latency, CancellationToken token)
    {
        var enumerator = new MMDeviceEnumerator();
        var device = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active)
           .FirstOrDefault(wasapi => wasapi.FriendlyName == deviceName);

        if (device == null)
        {
            Logging.At(this).Error("Selected output device does not exist");
            return;
        }

        OutputDevice = device;

        WasapiOut wo;
        try
        {
            wo = new WasapiOut(device, AudioClientShareMode.Exclusive, true, latency);
            wo.Init(this);
            wo.Play();

            Logging.At(this).Information("Outputting to '{Output}' with {Latency}ms latency", device.FriendlyName, latency);
            Latency = latency;
        }
        catch (Exception ex)
        {
            Logging.At(this).Error("Failed to output to '{DeviceName}', likely cannot get WASAPI exclusive mode: {ExMessage}", deviceName,
                                   ex.Message);
            return;
        }

        while (wo.PlaybackState == PlaybackState.Playing)
        {
            Thread.Sleep(10);
            if (token.IsCancellationRequested)
                break;
        }

        wo.Stop();
        wo.Dispose();
    }

    public static IEnumerable<MMDevice> GetOutputDevices()
    {
        return new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
    }

    public void UpdateSettings(Settings.Settings settings)
    {
        bool  switchedState = settings.Master.Enabled != Enabled;
        float previousdB    = MasterVolume.dB;

        Enabled      = settings.Master.Enabled;
        MasterVolume = new Volume(settings.Master.Volume);

        if (switchedState || previousdB != MasterVolume.dB)
            Logging.At(this).Debug("{State}: {Rel}dB", Enabled ? "Enabled" : "Disabled", MasterVolume.dB);

        foreach (var sampler in _samplers)
        {
            sampler.UpdateSettings(settings);
        }
    }

    public int Read(float[] buffer, int offset, int count)
    {
        int samples = count;

        if (Enabled)
            samples = _chainEnd.Read(buffer, offset, count);

        Clock += (uint)samples;

        return samples;
    }

    private class ChainStart : ISampleProvider
    {
        public WaveFormat WaveFormat { get; }

        public ChainStart(WaveFormat waveFormat)
        {
            WaveFormat = waveFormat;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            return count;
        }
    }
}