using MechShakerEngine.Events;
using MechShakerEngine.Generators;
using NAudio.Wave;

namespace MechShakerEngine.Effects;

internal class LaserDamage : Effect<DamagedEvent>
{
    private readonly WaveGenerator _waveGenerator    = new(false);
    private readonly WaveGenerator _secWaveGenerator = new(false);

    private const float DPSFactor  = 10.0f;
    private const float Transition = 0.025f;
    private const float TickTime   = 0.5f;
    private const uint  TickCycles = (uint)(TickTime * Audio.SampleRate);

    private float _freq;
    private float _secFreq;
    private float _secAmp;
    private float _amplitudeExponent;

    private readonly record struct LaserInstance(float DPS, uint Expires);

    private readonly List<LaserInstance> _instances = new();

    public LaserDamage(ISampleProvider source, Audio audio) : base(source, audio)
    {
    }

    public override void Clear()
    {
        _instances.Clear();
        _waveGenerator.SetTarget(_freq, 0, Transition);
        _secWaveGenerator.SetTarget(_secFreq, 0, Transition);
    }

    protected override void Write(float[] buffer, int offset, int count, ref float activeInstances)
    {
        activeInstances += _instances.Count;

        uint  bufferEndClock = Audio.Clock + (uint)count;
        float amp            = 0;
        for (int i = 0; i < _instances.Count; i++)
        {
            var instance = _instances[i];
            amp += MathF.Pow(MathF.Min(instance.DPS / DPSFactor, 1), _amplitudeExponent) * Volume.Amplitude;
            if (instance.Expires < bufferEndClock)
                _instances.RemoveAt(i--);
        }

        amp *= Volume.Amplitude;
        float secAmp = _secAmp * amp;

        _waveGenerator.SetTarget(_freq, amp, Transition);
        _secWaveGenerator.SetTarget(_secFreq, secAmp, Transition);

        _waveGenerator.Write(buffer, offset, count, ref activeInstances);
        _secWaveGenerator.Write(buffer, offset, count, ref activeInstances);
    }

    protected override void OnEventDataReceived(DamagedEvent e)
    {
        float dps = e.Damage / TickTime;
        _instances.Add(new LaserInstance(dps, Audio.Clock + TickCycles));
    }

    protected override void OnUpdateSettings(Settings.Settings s)
    {
        UpdateVolume(s.LaserDamage);
        _freq              = s.LaserDamage.Frequency;
        _secFreq           = s.LaserDamage.SecondaryFrequency;
        _secAmp            = s.LaserDamage.SecondaryAmplitude / 100f;
        _amplitudeExponent = s.LaserDamage.AmplitudeExponent;
    }
}