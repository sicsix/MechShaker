using MechShakerEngine.Events;
using MechShakerEngine.Generators;
using NAudio.Wave;

namespace MechShakerEngine.Effects;

internal class Powering : Effect<PoweringEvent>
{
    private readonly List<RampGenerator> _generators = new();

    private const float TransitionTime = 0.4f;

    private float _startFreq;
    private float _endFreq;
    private float _startAmp;
    private float _secFreqFactor;
    private float _secAmp;

    private float _initDelay;
    private float _initLength;
    private float _poweringUpDelay;
    private float _poweringUpLength;
    private float _shuttingDownDelay;
    private float _shuttingDownLength;

    public Powering(ISampleProvider source, Audio audio) : base(source, audio)
    {
    }

    public override void Clear()
    {
        // Will potentially cause a pop, however this will prevent the FX from playing after destruction
        _generators.Clear();
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

    protected override void OnEventDataReceived(PoweringEvent e)
    {
        if (e.Initialising)
        {
            float startAmp    = Volume.Amplitude * _startAmp;
            float endAmp      = Volume.Amplitude;
            float secStartAmp = startAmp * _secAmp;
            float secEndAmp   = endAmp   * _secAmp;
            _generators.Add(new RampGenerator(_startFreq, _endFreq, startAmp, endAmp, _initLength, TransitionTime, TransitionTime,
                                              _initDelay, Audio.Clock, false));
            _generators.Add(new RampGenerator(_startFreq * _secFreqFactor, _endFreq * _secFreqFactor, secStartAmp, secEndAmp, _initLength,
                                              TransitionTime, TransitionTime, _initDelay, Audio.Clock, false));
        }
        else if (e.PoweringUp)
        {
            float startAmp    = Volume.Amplitude * _startAmp;
            float endAmp      = Volume.Amplitude;
            float secStartAmp = startAmp * _secAmp;
            float secEndAmp   = endAmp   * _secAmp;
            _generators.Add(new RampGenerator(_startFreq, _endFreq, startAmp, endAmp, _poweringUpLength, TransitionTime, TransitionTime,
                                              _poweringUpDelay, Audio.Clock, false));
            _generators.Add(new RampGenerator(_startFreq * _secFreqFactor, _endFreq * _secFreqFactor, secStartAmp, secEndAmp,
                                              _poweringUpLength, TransitionTime, TransitionTime, _poweringUpDelay, Audio.Clock, false));
        }
        else if (e.ShuttingDown)
        {
            float startAmp    = Volume.Amplitude * _startAmp;
            float endAmp      = Volume.Amplitude;
            float secStartAmp = startAmp * _secAmp;
            float secEndAmp   = endAmp   * _secAmp;
            _generators.Add(new RampGenerator(_endFreq, _startFreq, endAmp, startAmp, _shuttingDownLength, TransitionTime, TransitionTime,
                                              _shuttingDownDelay, Audio.Clock, false));
            _generators.Add(new RampGenerator(_startFreq * _secFreqFactor, _endFreq * _secFreqFactor, secEndAmp, secStartAmp,
                                              _shuttingDownLength, TransitionTime, TransitionTime, _shuttingDownDelay, Audio.Clock, false));
        }
    }

    protected override void OnUpdateSettings(Settings.Settings s)
    {
        UpdateVolume(s.Powering);
        _startFreq          = s.Powering.StartFrequency;
        _endFreq            = s.Powering.EndFrequency;
        _startAmp           = s.Powering.StartAmplitude           / 100f;
        _secFreqFactor      = s.Powering.SecondaryFrequencyFactor / 100f;
        _secAmp             = s.Powering.SecondaryAmplitude       / 100f;
        _initDelay          = s.Powering.InitDelay                / 1000f;
        _initLength         = s.Powering.InitLength               / 1000f;
        _poweringUpDelay    = s.Powering.PoweringUpDelay          / 1000f;
        _poweringUpLength   = s.Powering.PoweringUpLength         / 1000f;
        _shuttingDownDelay  = s.Powering.ShuttingDownDelay        / 1000f;
        _shuttingDownLength = s.Powering.ShuttingDownLength       / 1000f;
    }
}