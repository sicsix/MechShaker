using MechShakerEngine.Events;
using MechShakerEngine.Generators;
using MechShakerEngine.Interfaces;
using NAudio.Wave;

namespace MechShakerEngine.Effects;

internal class AutocannonsAndRifles : Effect<ProjectileEvent>
{
    private readonly List<IGenerator> _generators = new();

    private const float MinFreqFactor = 8f;
    private const float MaxFreqFactor = 343f;
    private const float MinImpulse    = 350f;
    private const float MaxImpulse    = 6000f;

    private float _minFreq;
    private float _maxFreq;
    private float _minAmp;
    private float _minLength;
    private float _maxLength;
    private float _attackEnd;
    private float _decayStart;

    public AutocannonsAndRifles(ISampleProvider source, Audio audio) : base(source, audio)
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

    protected override void OnEventDataReceived(ProjectileEvent e)
    {
        float totalImpulse = e.Impulse * e.NumberOfProjectiles;
        float freqFactor   = e.Speed   / totalImpulse;

        float freq = MathF.Sqrt(MathF.Sqrt((freqFactor - MinFreqFactor) / (MaxFreqFactor - MinFreqFactor))) * (_maxFreq - _minFreq)
                   + _minFreq;
        float amp    = (totalImpulse - MinImpulse) / (MaxImpulse - MinImpulse)             * (1f         - _minAmp)    + _minAmp;
        float length = MathF.Sqrt((totalImpulse - MinImpulse) / (MaxImpulse - MinImpulse)) * (_maxLength - _minLength) + _minLength;

        int cycles = CalculateCycles(freq, length);
        (int attackEnd, int decayStart) = CalculateAttackAndDecayCycles(freq, length, _attackEnd, _decayStart);

        amp *= Volume.Amplitude;

        int count = Math.Max(1, e.NumberOfTimesToFire);

        if (count == 1)
        {
            _generators.Add(new ImpulseGenerator(freq, amp, cycles, attackEnd, decayStart, Audio.Clock));
            return;
        }

        _generators.Add(new DummyGenerator(length + e.DelayBetweenFiring * (count - 1)));

        for (int i = 0; i < count; i++)
        {
            float delay  = i * e.DelayBetweenFiring;
            uint  fireAt = (uint)(delay * Audio.SampleRate + Audio.Clock);
            _generators.Add(new ImpulseGenerator(freq, amp, cycles, attackEnd, decayStart, Audio.Clock, fireAt, false));
        }
    }

    protected override void OnUpdateSettings(Settings.Settings s)
    {
        UpdateVolume(s.AutocannonsAndRifles);
        _minFreq    = s.AutocannonsAndRifles.MinFrequency;
        _maxFreq    = s.AutocannonsAndRifles.MaxFrequency;
        _minAmp     = s.AutocannonsAndRifles.MinAmplitude / 100f;
        _minLength  = s.AutocannonsAndRifles.MinLength    / 1000f;
        _maxLength  = s.AutocannonsAndRifles.MaxLength    / 1000f;
        _attackEnd  = s.AutocannonsAndRifles.AttackEnd    / 100f;
        _decayStart = s.AutocannonsAndRifles.DecayStart   / 100f;
    }
}