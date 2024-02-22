using MechShakerEngine.Events;
using MechShakerEngine.Generators;
using NAudio.Wave;

namespace MechShakerEngine.Effects;

internal class Melee : Effect<MeleeEvent>
{
    private readonly List<ImpulseGenerator> _generators = new();

    private const int MinIntervalMs = 200;

    private float _hitFreq;
    private int   _hitCycles;
    private int   _hitAttackEnd;
    private int   _hitDecayStart;

    private float _swingFreq;
    private int   _swingCycles;
    private int   _swingAttackEnd;
    private int   _swingDecayStart;
    private float _swingAmp;

    private float _massFactor;

    private DateTime _lastHitTime = DateTime.MinValue;

    public Melee(ISampleProvider source, Audio audio) : base(source, audio)
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

    protected override void OnEventDataReceived(MeleeEvent e)
    {
        float       massRatio     = Math.Clamp((e.MassInTons - 20f) / 80f, 0f, 1f);
        float       massMinAmp    = _massFactor;
        const float massMaxAmp    = 1.0f;
        float       massAmpEffect = Lerp(massMinAmp, massMaxAmp, massRatio);

        if (e.IsHit)
        {
            var now = DateTime.UtcNow;
            if ((now - _lastHitTime).TotalMilliseconds < MinIntervalMs)
                return;
            _lastHitTime = now;

            float amp = Volume.Amplitude * massAmpEffect;
            _generators.Add(new ImpulseGenerator(_hitFreq, amp, _hitCycles, _hitAttackEnd, _hitDecayStart, Audio.Clock,
                                                 canBeActiveInstance: false));
        }
        else
        {
            float amp = Volume.Amplitude * massAmpEffect * _swingAmp;
            _generators.Add(new ImpulseGenerator(_swingFreq, amp, _swingCycles, _swingAttackEnd, _swingDecayStart, Audio.Clock));
        }
    }

    protected override void OnUpdateSettings(Settings.Settings s)
    {
        UpdateVolume(s.Melee);
        _hitFreq = s.Melee.HitFrequency;
        float hitLength     = s.Melee.HitLength     / 1000f;
        float hitAttackEnd  = s.Melee.HitAttackEnd  / 100f;
        float hitDecayStart = s.Melee.HitDecayStart / 100f;
        _hitCycles                      = CalculateCycles(_hitFreq, hitLength);
        (_hitAttackEnd, _hitDecayStart) = CalculateAttackAndDecayCycles(_hitFreq, hitLength, hitAttackEnd, hitDecayStart);

        _swingFreq = s.Melee.SwingFrequency;
        float swingLength     = s.Melee.HitLength     / 1000f;
        float swingAttackEnd  = s.Melee.HitAttackEnd  / 100f;
        float swingDecayStart = s.Melee.HitDecayStart / 100f;
        _swingCycles                        = CalculateCycles(_swingFreq, swingLength);
        (_swingAttackEnd, _swingDecayStart) = CalculateAttackAndDecayCycles(_swingFreq, swingLength, swingAttackEnd, swingDecayStart);
        _swingAmp                           = s.Melee.SwingAmplitude / 100f;

        _massFactor = s.Melee.MassFactor / 100f;
    }
}