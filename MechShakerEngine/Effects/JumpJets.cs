using MechShakerEngine.Events;
using MechShakerEngine.Generators;
using NAudio.Wave;

namespace MechShakerEngine.Effects;

internal class JumpJets : Effect<JumpJetsEvent>
{
    private readonly WaveGenerator _waveGenerator     = new(true);
    private readonly WaveGenerator _secWaveGenerator  = new(false);
    private readonly WaveGenerator _tertWaveGenerator = new(false);

    private float _freq;
    private float _secFreq;
    private float _secAmp;
    private float _tertFreq;
    private float _tertAmp;
    private float _transitionOnTime;
    private float _transitionOffTime;

    public JumpJets(ISampleProvider source, Audio audio) : base(source, audio)
    {
    }

    public override void Clear()
    {
        _waveGenerator.SetTarget(_freq, 0, _transitionOffTime);
        _secWaveGenerator.SetTarget(_secFreq, 0, _transitionOffTime);
        _tertWaveGenerator.SetTarget(_tertFreq, 0, _transitionOffTime);
    }

    protected override void Write(float[] buffer, int offset, int count, ref float activeInstances)
    {
        _waveGenerator.Write(buffer, offset, count, ref activeInstances);
        _secWaveGenerator.Write(buffer, offset, count, ref activeInstances);
        _tertWaveGenerator.Write(buffer, offset, count, ref activeInstances);
    }

    protected override void OnEventDataReceived(JumpJetsEvent e)
    {
        if (e.Active)
        {
            float amp     = Volume.Amplitude;
            float secAmp  = amp * _secAmp;
            float tertAmp = amp * _tertAmp;

            _waveGenerator.SetTarget(_freq, amp, _transitionOnTime);
            _secWaveGenerator.SetTarget(_secFreq, secAmp, _transitionOnTime);
            _tertWaveGenerator.SetTarget(_tertFreq, tertAmp, _transitionOnTime);
        }
        else
        {
            _waveGenerator.SetTarget(_freq, 0, _transitionOffTime);
            _secWaveGenerator.SetTarget(_secFreq, 0, _transitionOffTime);
            _tertWaveGenerator.SetTarget(_tertFreq, 0, _transitionOffTime);
        }
    }

    protected override void OnUpdateSettings(Settings.Settings s)
    {
        UpdateVolume(s.JumpJets);
        _freq              = s.JumpJets.Frequency;
        _secFreq           = s.JumpJets.SecondaryFrequency;
        _secAmp            = s.JumpJets.SecondaryAmplitude / 100f;
        _tertFreq          = s.JumpJets.TertiaryFrequency;
        _tertAmp           = s.JumpJets.TertiaryAmplitude / 100f;
        _transitionOnTime  = s.JumpJets.TransitionOnTime  / 1000f;
        _transitionOffTime = s.JumpJets.TransitionOffTime / 1000f;
    }
}