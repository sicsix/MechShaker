using MechShakerEngine.Events;
using MechShakerEngine.Generators;
using NAudio.Wave;

namespace MechShakerEngine.Effects;

internal class Footsteps : Effect<FootstepEvent>
{
    private readonly List<ImpulseGenerator> _generators = new();

    private const int MinIntervalMs = 200;

    private float _freq;
    private int   _cycles;
    private int   _attackEnd;
    private int   _decayStart;
    private float _speedFactor;
    private float _massFactor;

    private DateTime _lastEventTime = DateTime.MinValue;

    public Footsteps(ISampleProvider source, Audio audio) : base(source, audio)
    {
    }

    public override void Clear()
    {
    }

    protected override void Write(float[] buffer, int offset, int count, ref float activeInstances)
    {
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

    protected override void OnEventDataReceived(FootstepEvent e)
    {
        var now = DateTime.UtcNow;
        if ((now - _lastEventTime).TotalMilliseconds < MinIntervalMs)
            return;
        _lastEventTime = now;

        const float minSpeed       = 16f;
        const float maxSpeed       = 150f;
        float       speedRatio     = Math.Clamp((e.SpeedInKmh + minSpeed) / (maxSpeed + minSpeed), 0f, 1f);
        float       speedMinAmp    = _speedFactor;
        const float speedMaxAmp    = 1.0f;
        float       speedAmpEffect = Lerp(speedMaxAmp, speedMinAmp, speedRatio);


        float       massRatio     = Math.Clamp((e.MassInTons - 20f) / 80f, 0f, 1f);
        float       massMinAmp    = _massFactor;
        const float massMaxAmp    = 1.0f;
        float       massAmpEffect = Lerp(massMinAmp, massMaxAmp, massRatio);

        float amp = Volume.Amplitude * speedAmpEffect * massAmpEffect;
        _generators.Add(new ImpulseGenerator(_freq, amp, _cycles, _attackEnd, _decayStart, Audio.Clock));
    }

    protected override void OnUpdateSettings(Settings.Settings s)
    {
        UpdateVolume(s.Footsteps);
        _freq        = s.Footsteps.Frequency;
        _speedFactor = s.Footsteps.SpeedFactor / 100f;
        _massFactor  = s.Footsteps.MassFactor  / 100f;
        float length     = s.Footsteps.Length     / 1000f;
        float attackEnd  = s.Footsteps.AttackEnd  / 100f;
        float decayStart = s.Footsteps.DecayStart / 100f;
        _cycles                   = CalculateCycles(_freq, length);
        (_attackEnd, _decayStart) = CalculateAttackAndDecayCycles(_freq, length, attackEnd, decayStart);
    }
}