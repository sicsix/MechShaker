using MechShakerEngine.Interfaces;
using NAudio.Wave;

namespace MechShakerEngine;

internal class EffectGroup : ISampleProvider
{
    public           WaveFormat      WaveFormat { get; }
    private readonly ISampleProvider _source;
    private readonly List<IEffect>   _effects     = new();
    private          float[]         _groupBuffer = Array.Empty<float>();

    private const float A              = 0.4f; // Sensitivity of the scale
    private const float B              = 2;    // Rate of growth of the volume increase
    private       float _scalingFactor = 1.0f;

    public EffectGroup(ISampleProvider source)
    {
        WaveFormat = source.WaveFormat;
        _source    = source;
    }

    public void AddEffect<T>(Effect<T> effect) where T : IEvent
    {
        _effects.Add(effect);
    }

    public int Read(float[] buffer, int offset, int count)
    {
        if (_groupBuffer.Length < buffer.Length)
            _groupBuffer = new float[buffer.Length];

        Array.Clear(_groupBuffer);

        float activeInstances = 0;

        for (int i = 0; i < _effects.Count; i++)
        {
            _effects[i].WriteToGroup(_groupBuffer, offset, count);
            activeInstances += _effects[i].GetActiveInstances();
        }

        int samples = _source.Read(buffer, offset, count);

        activeInstances = Math.Max(activeInstances, 1);

        float goalVolume       = 1f + A * MathF.Log(activeInstances, B);
        float newScalingFactor = goalVolume / activeInstances;

        int samplesFor5ms = Math.Min((int)(Audio.SampleRate * 0.005f), samples);

        for (int i = 0; i < samplesFor5ms; i++)
        {
            float bufferPositionPct = (float)i / samplesFor5ms;
            buffer[offset + i] += _groupBuffer[offset + i]
                                * (_scalingFactor * (1 - bufferPositionPct) + newScalingFactor * bufferPositionPct);
        }

        _scalingFactor = newScalingFactor;

        for (int i = samplesFor5ms; i < samples; i++)
        {
            buffer[offset + i] += _groupBuffer[offset + i] * _scalingFactor;
        }

        return samples;
    }
}