using MechShakerEngine.Events;
using MechShakerEngine.Generators;
using NAudio.Wave;

namespace MechShakerEngine.Effects;

internal class MASC : Effect<MASCEvent>
{
    private readonly WaveGenerator _waveGenerator    = new(false);
    private readonly WaveGenerator _secWaveGenerator = new(false);

    private const float TransitionTime = 0.05f;

    private float _freq;
    private float _secFreq;
    private float _secAmp;
    private float _gaugeExponent;

    private bool _engaged;

    public MASC(ISampleProvider source, Audio audio) : base(source, audio)
    {
    }

    public override void Clear()
    {
        _waveGenerator.SetTarget(_freq, 0, TransitionTime);
        _secWaveGenerator.SetTarget(_secFreq, 0, TransitionTime);
    }

    protected override void Write(float[] buffer, int offset, int count, ref float activeInstances)
    {
        _waveGenerator.Write(buffer, offset, count, ref activeInstances);
        _secWaveGenerator.Write(buffer, offset, count, ref activeInstances);
    }

    protected override void OnEventDataReceived(MASCEvent e)
    {
        if (e.IsEngaged && !_engaged && e.GaugeValue == 0)
        {
            // TODO on start from 0. Might need to store previous gauge value?   
        }
        else if (!e.IsEngaged && _engaged)
        {
            // TODO do stop
        }

        _engaged = e.IsEngaged;

        float amp    = Volume.Amplitude * MathF.Pow(e.GaugeValue, _gaugeExponent);
        float secAmp = amp              * _secAmp;

        _waveGenerator.SetTarget(_freq, amp, TransitionTime);
        _secWaveGenerator.SetTarget(_secFreq, secAmp, TransitionTime);
    }

    protected override void OnUpdateSettings(Settings.Settings s)
    {
        UpdateVolume(s.MASC);
        _freq          = s.MASC.Frequency;
        _secFreq       = s.MASC.SecondaryFrequency;
        _secAmp        = s.MASC.SecondaryAmplitude / 100f;
        _gaugeExponent = s.MASC.GaugeExponent;
    }
}