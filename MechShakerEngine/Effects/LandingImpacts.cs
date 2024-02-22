using MechShakerEngine.Events;
using MechShakerEngine.Generators;
using NAudio.Wave;

namespace MechShakerEngine.Effects;

internal class LandingImpacts : Effect<LandedEvent>
{
    private readonly List<ImpulseGenerator> _generators = new();

    private const float MaxForce = 2000f;

    private float _freq;
    private int   _cycles;
    private int   _attackEnd;
    private int   _decayStart;

    public LandingImpacts(ISampleProvider source, Audio audio) : base(source, audio)
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

    protected override void OnEventDataReceived(LandedEvent e)
    {
        float mass         = e.MassInTons         * 1000f;
        float acceleration = e.AccelerationInKmh2 * (1000f / (3600f * 3600f));
        float force        = mass                 * acceleration;

        force = MathF.Min(Math.Abs(force), MaxForce);
        float forceFactor = MathF.Sqrt(force / MaxForce);
        float amp         = Volume.Amplitude * forceFactor;

        _generators.Add(new ImpulseGenerator(_freq, amp, _cycles, _attackEnd, _decayStart, Audio.Clock));
    }

    protected override void OnUpdateSettings(Settings.Settings s)
    {
        UpdateVolume(s.LandingImpacts);
        _freq = s.LandingImpacts.Frequency;
        float length     = s.LandingImpacts.Length     / 1000f;
        float attackEnd  = s.LandingImpacts.AttackEnd  / 100f;
        float decayStart = s.LandingImpacts.DecayStart / 100f;
        _cycles                   = CalculateCycles(_freq, length);
        (_attackEnd, _decayStart) = CalculateAttackAndDecayCycles(_freq, length, attackEnd, decayStart);
    }
}