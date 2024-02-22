using MechShakerEngine.Events;
using MechShakerEngine.Generators;
using NAudio.Wave;

namespace MechShakerEngine.Effects;

internal class ProjectileDamage : Effect<DamagedEvent>
{
    private readonly List<ImpulseGenerator> _generators      = new();
    private readonly List<ImpulseGenerator> _burstGenerators = new();

    private const float DamageCap             = 20f;
    private const float BurstDamageCutoff     = 3f;
    private const float InstanceRatioForBurst = 1 / 3f;

    private float _freq;
    private float _attackEnd;
    private float _decayStart;
    private float _maxLength;
    private float _ampExponent;
    private float _lengthExponent;

    public ProjectileDamage(ISampleProvider source, Audio audio) : base(source, audio)
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

        // This hacky workaround is because we can't identify weapon component source, only weapon type
        // But we can group the weapons by damage to filter for burst and AC2s and make these count less to overall instance count
        for (int i = 0; i < _burstGenerators.Count; i++)
        {
            if (_burstGenerators[i].Complete)
            {
                _burstGenerators.RemoveAt(i--);
                continue;
            }

            _burstGenerators[i].Write(buffer, offset, count, ref activeInstances);
        }

        activeInstances += _burstGenerators.Count * InstanceRatioForBurst;
    }

    protected override void OnEventDataReceived(DamagedEvent e)
    {
        float damageRatio  = MathF.Min(e.Damage / DamageCap, 1f);
        float ampFactor    = MathF.Pow(damageRatio, _ampExponent);
        float lengthFactor = MathF.Pow(damageRatio, _lengthExponent);

        float amp    = Volume.Amplitude * ampFactor;
        float length = _maxLength       * lengthFactor;

        int cycles = CalculateCycles(_freq, length);
        (int attackEnd, int decayStart) = CalculateAttackAndDecayCycles(_freq, length, _attackEnd, _decayStart);

        if (e.Damage > BurstDamageCutoff)
            _generators.Add(new ImpulseGenerator(_freq, amp, cycles, attackEnd, decayStart, Audio.Clock));
        else
            _burstGenerators.Add(new ImpulseGenerator(_freq, amp, cycles, attackEnd, decayStart, Audio.Clock, canBeActiveInstance: false));
    }

    protected override void OnUpdateSettings(Settings.Settings s)
    {
        UpdateVolume(s.ProjectileDamage);
        _freq           = s.ProjectileDamage.Frequency;
        _attackEnd      = s.ProjectileDamage.AttackEnd  / 100f;
        _decayStart     = s.ProjectileDamage.DecayStart / 100f;
        _maxLength      = s.ProjectileDamage.MaxLength  / 1000f;
        _ampExponent    = s.ProjectileDamage.AmplitudeExponent;
        _lengthExponent = s.ProjectileDamage.LengthExponent;
    }
}