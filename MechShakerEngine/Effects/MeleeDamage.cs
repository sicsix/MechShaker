﻿using MechShakerEngine.Events;
using MechShakerEngine.Generators;
using NAudio.Wave;

namespace MechShakerEngine.Effects;

internal class MeleeDamage : Effect<DamagedEvent>
{
    private readonly List<ImpulseGenerator> _generators = new();

    private const float DamageCap = 80f;

    private float _freq;
    private float _attackEnd;
    private float _decayStart;
    private float _maxLength;
    private float _ampExponent;
    private float _lengthExponent;

    public MeleeDamage(ISampleProvider source, Audio audio) : base(source, audio)
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

    protected override void OnEventDataReceived(DamagedEvent e)
    {
        float damageRatio  = MathF.Min(e.Damage / DamageCap, 1f);
        float ampFactor    = MathF.Pow(damageRatio, _ampExponent);
        float lengthFactor = MathF.Pow(damageRatio, _lengthExponent);

        float amp    = Volume.Amplitude * ampFactor;
        float length = _maxLength       * lengthFactor;

        int cycles = CalculateCycles(_freq, length);
        (int attackEnd, int decayStart) = CalculateAttackAndDecayCycles(_freq, length, _attackEnd, _decayStart);

        _generators.Add(new ImpulseGenerator(_freq, amp, cycles, attackEnd, decayStart, Audio.Clock));
    }

    protected override void OnUpdateSettings(Settings.Settings s)
    {
        UpdateVolume(s.MeleeDamage);
        _freq           = s.MeleeDamage.Frequency;
        _attackEnd      = s.MeleeDamage.AttackEnd  / 100f;
        _decayStart     = s.MeleeDamage.DecayStart / 100f;
        _maxLength      = s.MeleeDamage.MaxLength  / 1000f;
        _ampExponent    = s.MeleeDamage.AmplitudeExponent;
        _lengthExponent = s.MeleeDamage.LengthExponent;
    }
}