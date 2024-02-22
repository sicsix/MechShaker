using System.Runtime.CompilerServices;
using MechShakerEngine.Interfaces;

namespace MechShakerEngine.Generators;

internal class RampGenerator : IGenerator
{
    public bool Complete { get; private set; }

    private readonly float _startFrequency;
    private readonly float _endFrequency;
    private readonly float _frequencyChange;
    private readonly float _startAmplitude;
    private readonly float _endAmplitude;
    private readonly float _amplitudeChange;
    private readonly int   _onSamples;
    private readonly int   _rampSamples;
    private readonly int   _offSamples;
    private readonly uint  _startAtClock;
    private readonly bool  _canBeActiveInstance;

    private uint  _clock;
    private float _currentFrequency;
    private float _theta;
    private int   _sampleIndex;
    private int   _samplesRemaining;

    public RampGenerator(float startFrequency,
                         float endFrequency,
                         float startAmplitude,
                         float endAmplitude,
                         float length,
                         float transitionOnTime,
                         float transitionOffTime,
                         float delay,
                         uint  clock,
                         bool  canBeActiveInstance = true)
    {
        _startFrequency      = startFrequency;
        _endFrequency        = endFrequency;
        _frequencyChange     = endFrequency - startFrequency;
        _startAmplitude      = startAmplitude;
        _endAmplitude        = endAmplitude;
        _amplitudeChange     = endAmplitude - startAmplitude;
        _onSamples           = (int)(transitionOnTime  * Audio.SampleRate);
        _rampSamples         = (int)(length            * Audio.SampleRate);
        _offSamples          = (int)(transitionOffTime * Audio.SampleRate);
        _sampleIndex         = 0;
        _samplesRemaining    = _onSamples + _rampSamples + _offSamples;
        _clock               = clock;
        _startAtClock        = clock + (uint)(delay * Audio.SampleRate);
        _canBeActiveInstance = canBeActiveInstance;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public void Write(float[] buffer, int offset, int count, ref float activeInstances)
    {
        Complete = _samplesRemaining == 0;
        if (Complete)
            return;

        int writeStart = Math.Max((int)(_startAtClock - _clock), 0);
        int writeEnd   = Math.Min(count, writeStart + _samplesRemaining);

        _clock += (uint)count;

        if (writeStart >= writeEnd)
            return;

        if (_canBeActiveInstance)
            activeInstances += 1;

        float onReciprocal   = 1f / _onSamples;
        float rampReciprocal = 1f / _rampSamples;
        float offReciprocal  = 1f / _offSamples;

        float thetaIncrement = _currentFrequency * (MathF.PI * 2) / Audio.SampleRate;

        for (int i = writeStart; i < writeEnd; i++)
        {
            int onRemaining   = _onSamples        - _sampleIndex;
            int rampRemaining = _samplesRemaining - _offSamples;

            float amplitude;
            if (onRemaining > 0)
            {
                _currentFrequency = _startFrequency;
                amplitude         = _sampleIndex * onReciprocal * _startAmplitude;
            }
            else if (rampRemaining > 0)
            {
                float rampPos = 1 - rampRemaining * rampReciprocal;
                _currentFrequency = _startFrequency + _frequencyChange * rampPos;
                amplitude         = _startAmplitude + _amplitudeChange * rampPos;
            }
            else
            {
                _currentFrequency = _endFrequency;
                amplitude         = _samplesRemaining * offReciprocal * _endAmplitude;
            }

            float value = buffer[offset + i];
            value              += amplitude * MathF.Sin(_theta);
            buffer[offset + i] =  value;

            _theta += thetaIncrement;
            _theta %= MathF.PI * 2;

            thetaIncrement = _currentFrequency * (MathF.PI * 2) / Audio.SampleRate;

            _sampleIndex++;
            _samplesRemaining--;
        }

        if (_samplesRemaining <= 0)
            Complete = true;
    }
}