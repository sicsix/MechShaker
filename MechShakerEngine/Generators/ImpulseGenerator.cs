using System.Runtime.CompilerServices;
using MechShakerEngine.Interfaces;

namespace MechShakerEngine.Generators;

internal class ImpulseGenerator : IGenerator
{
    public bool Complete { get; private set; }

    private readonly float _amplitude;
    private readonly int   _samplesPerCycle;
    private readonly int   _attackSamples;
    private readonly int   _releaseSamples;
    private readonly bool  _canBeActiveInstance;

    private          uint _clock;
    private readonly uint _startAtClock;
    private          int  _theta;
    private          int  _sampleIndex;
    private          int  _samplesRemaining;

    public ImpulseGenerator(float frequency,
                            float amplitude,
                            int   cycles,
                            int   attackEndCycle,
                            int   decayStartCycle,
                            uint  clock,
                            uint  startAtClock        = 0,
                            bool  canBeActiveInstance = true)
    {
        _amplitude           = amplitude;
        _samplesPerCycle     = (int)(Audio.SampleRate / frequency);
        _attackSamples       = attackEndCycle             * _samplesPerCycle;
        _releaseSamples      = (cycles - decayStartCycle) * _samplesPerCycle;
        _sampleIndex         = 0;
        _samplesRemaining    = cycles * _samplesPerCycle;
        _canBeActiveInstance = canBeActiveInstance;
        _clock               = clock;
        _startAtClock        = startAtClock == 0 ? clock : startAtClock;
        _theta               = (int)(startAtClock % _samplesPerCycle);
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

        float attackReciprocal  = 1f            / _attackSamples;
        float releaseReciprocal = 1f            / _releaseSamples;
        float thetaReciprocal   = 2f * MathF.PI / _samplesPerCycle;

        for (int i = writeStart; i < writeEnd; i++)
        {
            int attackRemaining  = _attackSamples    - _sampleIndex;
            int sustainRemaining = _samplesRemaining - _releaseSamples;

            float amplitude;
            if (attackRemaining > 0)
                amplitude = _sampleIndex * attackReciprocal * _amplitude;
            else if (sustainRemaining > 0)
                amplitude = _amplitude;
            else
                amplitude = _samplesRemaining * releaseReciprocal * _amplitude;

            float value = buffer[i];

            float radians = _theta * thetaReciprocal;
            value += amplitude * MathF.Sin(radians);

            buffer[i] = value;
            _sampleIndex++;
            _samplesRemaining--;
            _theta++;
            if (_theta > _samplesPerCycle)
                _theta = 0;
        }

        if (_samplesRemaining <= 0)
            Complete = true;
    }
}