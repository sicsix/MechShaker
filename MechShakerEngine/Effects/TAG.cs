using MechShakerEngine.Events;
using MechShakerEngine.Generators;
using NAudio.Wave;

namespace MechShakerEngine.Effects;

internal class TAG : Effect<TraceEvent>
{
    private readonly WaveGenerator _waveGenerator    = new(false);
    private readonly WaveGenerator _secWaveGenerator = new(false);

    private readonly List<int> _activeTAGs = new();

    private const float TransitionTime = 0.05f;

    private float _freq;
    private float _secFreq;
    private float _secAmp;

    public TAG(ISampleProvider source, Audio audio) : base(source, audio)
    {
    }

    public override void Clear()
    {
        _waveGenerator.SetTarget(_freq, 0, TransitionTime);
        _secWaveGenerator.SetTarget(_secFreq, 0, TransitionTime);
        _activeTAGs.Clear();
    }

    protected override void Write(float[] buffer, int offset, int count, ref float activeInstances)
    {
        _waveGenerator.Write(buffer, offset, count, ref activeInstances);
        _secWaveGenerator.Write(buffer, offset, count, ref activeInstances);
        activeInstances += _activeTAGs.Count;
    }

    protected override void OnEventDataReceived(TraceEvent e)
    {
        if (e.Active)
        {
            int idx = _activeTAGs.IndexOf(e.WeaponId);
            if (idx == -1)
                _activeTAGs.Add(e.WeaponId);
        }
        else
            _activeTAGs.Remove(e.WeaponId);

        float amp    = Volume.Amplitude * _activeTAGs.Count;
        float secAmp = amp              * _secAmp;

        _waveGenerator.SetTarget(_freq, amp, TransitionTime);
        _secWaveGenerator.SetTarget(_secFreq, secAmp, TransitionTime);
    }

    protected override void OnUpdateSettings(Settings.Settings s)
    {
        UpdateVolume(s.TAG);
        _freq    = s.TAG.Frequency;
        _secFreq = s.TAG.SecondaryFrequency;
        _secAmp  = s.TAG.SecondaryAmplitude / 100f;
    }
}