using MechShakerEngine.Interfaces;

namespace MechShakerEngine.Generators;

internal class DummyGenerator : IGenerator
{
    public bool Complete { get; private set; }

    private int _samplesRemaining;

    public DummyGenerator(float length)
    {
        _samplesRemaining = (int)(length * Audio.SampleRate);
    }

    public void Write(float[] buffer, int offset, int count, ref float activeInstances)
    {
        if (_samplesRemaining > 0)
            activeInstances += 1;

        _samplesRemaining -= count;

        if (_samplesRemaining <= 0)
            Complete = true;
    }
}