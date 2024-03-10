using MechShakerEngine.Events;
using MechShakerEngine.Generators;
using NAudio.Wave;

namespace MechShakerEngine.Effects;

internal class Lasers : Effect<TraceEvent>
{
    private readonly List<ImpulseGenerator> _generators = new();

    private const float DPSFactor = 10.0f;

    private float _freq;
    private float _secFreq;
    private float _secAmp;
    private float _amplitudeExponent;

    public Lasers(ISampleProvider source, Audio audio) : base(source, audio)
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

    protected override void OnEventDataReceived(TraceEvent e)
    {
        float dps    = e.DamageOverDuration / e.Duration;
        float amp    = MathF.Pow(MathF.Min(dps / DPSFactor,1), _amplitudeExponent) * Volume.Amplitude;
        float secAmp = _secAmp * amp;

        int cycles = CalculateCycles(_freq, e.Duration);
        (int attackEnd, int decayStart) = CalculateAttackAndDecayCycles(_freq, e.Duration, 0, cycles);

        _generators.Add(new ImpulseGenerator(_freq, amp, cycles, attackEnd, decayStart, Audio.Clock));

        if (secAmp == 0)
            return;

        _generators.Add(new ImpulseGenerator(_secFreq, secAmp, cycles, attackEnd, decayStart, Audio.Clock, canBeActiveInstance: false));
    }

    protected override void OnUpdateSettings(Settings.Settings s)
    {
        UpdateVolume(s.Lasers);
        _freq           = s.Lasers.Frequency;
        _secFreq        = s.Lasers.SecondaryFrequency;
        _secAmp         = s.Lasers.SecondaryAmplitude / 100f;
        _amplitudeExponent = s.Lasers.AmplitudeExponent;
    }
}