using MechShakerEngine.Events;
using MechShakerEngine.Generators;
using NAudio.Wave;

namespace MechShakerEngine.Effects;

internal class Dropship : Effect<DropshipEvent>
{
    private readonly WaveGenerator          _flightWaveGenerator       = new(false);
    private readonly WaveGenerator          _flightSecWaveGenerator    = new(false);
    private readonly WaveGenerator          _flightTertWaveGenerator   = new(false);
    private readonly WaveGenerator          _turntableWaveGenerator    = new(false);
    private readonly WaveGenerator          _turntableSecWaveGenerator = new(false);
    private readonly List<ImpulseGenerator> _generators                = new();

    private const float FlightTransitionOn = 0.4f;

    private float _flightFreq;
    private float _flightAmp;
    private float _flightSecFreq;
    private float _flightSecAmp;
    private float _flightTertFreq;
    private float _flightTertAmp;
    private float _flightTransitionOff;

    private float _landedFreq;
    private float _landedAmp;
    private int   _landedCycles;
    private int   _landedAttackEnd;
    private int   _landedDecayStart;

    private float _turntableFreq;
    private float _turntableAmp;
    private float _turntableSecFreq;
    private float _turntableSecAmp;
    private float _turntableTransitionTime;

    public Dropship(ISampleProvider source, Audio audio) : base(source, audio)
    {
    }

    public override void Clear()
    {
        _flightWaveGenerator.SetTarget(_flightFreq, 0, _flightTransitionOff);
        _flightSecWaveGenerator.SetTarget(_flightSecFreq, 0, _flightTransitionOff);
        _flightTertWaveGenerator.SetTarget(_flightTertFreq, 0, _flightTransitionOff);
        _turntableWaveGenerator.SetTarget(_turntableFreq, 0, _turntableTransitionTime);
        _turntableSecWaveGenerator.SetTarget(_turntableSecFreq, 0, _turntableTransitionTime);
    }

    protected override void Write(float[] buffer, int offset, int count, ref float activeInstances)
    {
        _flightWaveGenerator.Write(buffer, offset, count, ref activeInstances);
        _flightSecWaveGenerator.Write(buffer, offset, count, ref activeInstances);
        _flightTertWaveGenerator.Write(buffer, offset, count, ref activeInstances);
        _turntableWaveGenerator.Write(buffer, offset, count, ref activeInstances);
        _turntableSecWaveGenerator.Write(buffer, offset, count, ref activeInstances);

        for (int i = 0; i < _generators.Count; i++)
        {
            if (_generators[i].Complete)
            {
                _generators.RemoveAt(i--);
                continue;
            }

            _generators[i].Write(buffer, offset, count, ref activeInstances);
        }
    }

    protected override void OnEventDataReceived(DropshipEvent e)
    {
        if (e.Flight)
        {
            float amp     = Volume.Amplitude * _flightAmp;
            float secAmp  = amp              * _flightSecAmp;
            float tertAmp = amp              * _flightTertAmp;
            _flightWaveGenerator.SetTarget(_flightFreq, amp, FlightTransitionOn);
            _flightSecWaveGenerator.SetTarget(_flightSecFreq, secAmp, FlightTransitionOn);
            _flightTertWaveGenerator.SetTarget(_flightTertFreq, tertAmp, FlightTransitionOn);
        }
        else if (e.Landed)
        {
            _flightWaveGenerator.SetTarget(_flightFreq, 0, _flightTransitionOff);
            _flightSecWaveGenerator.SetTarget(_flightSecFreq, 0, _flightTransitionOff);
            _flightTertWaveGenerator.SetTarget(_flightTertFreq, 0, _flightTransitionOff);

            float landedAmp = Volume.Amplitude * _landedAmp;
            _generators.Add(new ImpulseGenerator(_landedFreq, landedAmp, _landedCycles, _landedAttackEnd, _landedDecayStart, Audio.Clock,
                                                 canBeActiveInstance: false));
        }
        else if (e.TurntableStart)
        {
            float amp    = Volume.Amplitude * _turntableAmp;
            float secAmp = amp              * _turntableSecAmp;

            _turntableWaveGenerator.SetTarget(_turntableFreq, amp, _turntableTransitionTime);
            _turntableSecWaveGenerator.SetTarget(_turntableSecFreq, secAmp, _turntableTransitionTime);
        }
        else if (e.TurntableEnd)
        {
            _turntableWaveGenerator.SetTarget(_turntableFreq, 0, _turntableTransitionTime);
            _turntableSecWaveGenerator.SetTarget(_turntableSecFreq, 0, _turntableTransitionTime);
        }
    }

    protected override void OnUpdateSettings(Settings.Settings s)
    {
        UpdateVolume(s.Dropship);
        _flightFreq          = s.Dropship.FlightFrequency;
        _flightAmp           = s.Dropship.FlightAmplitude / 100f;
        _flightSecFreq       = s.Dropship.FlightSecondaryFrequency;
        _flightSecAmp        = s.Dropship.FlightSecondaryAmplitude / 100f;
        _flightTertFreq      = s.Dropship.FlightTertiaryFrequency;
        _flightTertAmp       = s.Dropship.FlightTertiaryAmplitude / 100f;
        _flightTransitionOff = s.Dropship.FlightTransitionOff     / 1000f;
        _landedFreq          = s.Dropship.LandedFrequency;
        _landedAmp           = s.Dropship.LandedAmplitude / 100f;
        float landedLength     = s.Dropship.LandedLength     / 1000f;
        float landedAttackEnd  = s.Dropship.LandedAttackEnd  / 100f;
        float landedDecayStart = s.Dropship.LandedDecayStart / 100f;
        _landedCycles                         = CalculateCycles(_landedFreq, landedLength);
        (_landedAttackEnd, _landedDecayStart) = CalculateAttackAndDecayCycles(_landedFreq, landedLength, landedAttackEnd, landedDecayStart);
        _turntableFreq                        = s.Dropship.TurntableFrequency;
        _turntableAmp                         = s.Dropship.TurntableAmplitude / 100f;
        _turntableSecFreq                     = s.Dropship.TurntableSecondaryFrequency;
        _turntableSecAmp                      = s.Dropship.TurntableSecondaryAmplitude / 100f;
        _turntableTransitionTime              = s.Dropship.TurntableTransitionTime     / 1000f;
    }
}