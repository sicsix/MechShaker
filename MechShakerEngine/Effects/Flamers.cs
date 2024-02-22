using MechShakerEngine.Events;
using MechShakerEngine.Generators;
using NAudio.Wave;

namespace MechShakerEngine.Effects;

internal class Flamers : Effect<TraceEvent>
{
    private readonly WaveGenerator _waveGenerator     = new(false);
    private readonly WaveGenerator _secWaveGenerator  = new(false);
    private readonly WaveGenerator _tertWaveGenerator = new(false);

    private readonly List<int> _activeFlamers = new();

    private const float TransitionTime = 0.1f;

    private int   _maxActiveFlamers;
    private float _freq;
    private float _secFreq;
    private float _secAmp;
    private float _tertFrequency;
    private float _tertAmplitude;

    public Flamers(ISampleProvider source, Audio audio) : base(source, audio)
    {
    }

    public override void Clear()
    {
        _waveGenerator.SetTarget(_freq, 0, TransitionTime);
        _secWaveGenerator.SetTarget(_secFreq, 0, TransitionTime);
        _tertWaveGenerator.SetTarget(_tertFrequency, 0, TransitionTime);
        _activeFlamers.Clear();
    }

    protected override void Write(float[] buffer, int offset, int count, ref float activeInstances)
    {
        _waveGenerator.Write(buffer, offset, count, ref activeInstances);
        _secWaveGenerator.Write(buffer, offset, count, ref activeInstances);
        _tertWaveGenerator.Write(buffer, offset, count, ref activeInstances);

        // This logic ensures the instance count doesn't drop to 0 while the generators are winding down, otherwise the volume will spike
        if (!_waveGenerator.IsActive)
            _maxActiveFlamers = 0;
        activeInstances += _maxActiveFlamers;
    }

    protected override void OnEventDataReceived(TraceEvent e)
    {
        if (e.Active)
        {
            int idx = _activeFlamers.IndexOf(e.WeaponId);
            if (idx == -1)
                _activeFlamers.Add(e.WeaponId);
        }
        else
            _activeFlamers.Remove(e.WeaponId);


        _maxActiveFlamers = Math.Max(_maxActiveFlamers, _activeFlamers.Count);

        float amp     = Volume.Amplitude * _activeFlamers.Count;
        float secAmp  = amp              * _secAmp;
        float tertAmp = amp              * _tertAmplitude;

        _waveGenerator.SetTarget(_freq, amp, TransitionTime);
        _secWaveGenerator.SetTarget(_secFreq, secAmp, TransitionTime);
        _tertWaveGenerator.SetTarget(_tertFrequency, tertAmp, TransitionTime);
    }

    protected override void OnUpdateSettings(Settings.Settings s)
    {
        UpdateVolume(s.Flamers);
        _freq          = s.Flamers.Frequency;
        _secFreq       = s.Flamers.SecondaryFrequency;
        _secAmp        = s.Flamers.SecondaryAmplitude / 100f;
        _tertFrequency = s.Flamers.TertiaryFrequency;
        _tertAmplitude = s.Flamers.TertiaryAmplitude / 100f;
    }
}