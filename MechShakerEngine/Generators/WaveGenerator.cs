using MechShakerEngine.Interfaces;

namespace MechShakerEngine.Generators;

internal class WaveGenerator : IGenerator
{
    public bool Complete => false;

    private float _currentFrequency;
    private float _targetFrequency;
    private float _rateOfChangeFrequency;

    private float _currentAmplitude;
    private float _targetAmplitude;
    private float _rateOfChangeAmplitude;

    private int _transitionSamplesRemaining;

    private readonly bool _canBeActiveInstance;

    private double _theta;

    public WaveGenerator(bool canBeActiveInstance)
    {
        _canBeActiveInstance = canBeActiveInstance;
    }

    public void SetTarget(float freq, float amplitude, float transitionTime)
    {
        _targetFrequency            = freq;
        _targetAmplitude            = amplitude;
        _transitionSamplesRemaining = (int)(transitionTime * Audio.SampleRate);
        _rateOfChangeFrequency      = (_targetFrequency - _currentFrequency) / _transitionSamplesRemaining;
        _rateOfChangeAmplitude      = (_targetAmplitude - _currentAmplitude) / _transitionSamplesRemaining;
    }

    public bool IsActive => !((_currentAmplitude == 0 && _targetAmplitude == 0) || (_currentFrequency == 0 && _targetFrequency == 0));

    public void Write(float[] buffer, int offset, int count, ref float activeInstances)
    {
        if (!IsActive)
            return;

        if (_canBeActiveInstance)
            activeInstances += 1;

        double thetaIncrement = _currentFrequency * (Math.PI * 2) / Audio.SampleRate;

        for (int i = 0; i < count; i++)
        {
            float value = buffer[offset + i];

            value += _currentAmplitude * MathF.Sin((float)_theta);

            buffer[offset + i] = value;

            _theta += thetaIncrement;
            _theta %= Math.PI * 2;

            if (_transitionSamplesRemaining == -1)
                continue;

            if (_transitionSamplesRemaining == 0)
            {
                _currentFrequency = _targetFrequency;
                _currentAmplitude = _targetAmplitude;
            }
            else
            {
                _currentFrequency += _rateOfChangeFrequency;
                _currentAmplitude += _rateOfChangeAmplitude;
            }

            thetaIncrement = _currentFrequency * (Math.PI * 2) / Audio.SampleRate;
            _transitionSamplesRemaining--;
        }
    }
}