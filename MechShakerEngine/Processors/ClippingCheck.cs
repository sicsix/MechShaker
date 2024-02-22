using MechShakerEngine.Interfaces;
using NAudio.Wave;

namespace MechShakerEngine.Processors;

internal class ClippingCheck : ISampleProvider, ISampler
{
    public WaveFormat WaveFormat { get; }

    private readonly ISampleProvider _source;

    private DateTime _lastWarningTime = DateTime.MinValue;

    public ClippingCheck(ISampleProvider source)
    {
        WaveFormat = source.WaveFormat;
        _source    = source;
    }

    public void Clear()
    {
    }

    public int Read(float[] buffer, int offset, int count)
    {
        int samples = _source.Read(buffer, offset, count);

        float max = 0;
        for (int i = 0; i < samples; i++)
        {
            float val = MathF.Abs(buffer[offset + i]);
            if (val > max)
                max = val;
        }

        if (max < 1.0f)
            return samples;

        var timeSinceLastWarning = DateTime.Now - _lastWarningTime;
        if (timeSinceLastWarning.TotalSeconds < 3)
            return samples;

        Logging.At(this).Warning("Output is clipping - {Max:0}%", max * 100f);
        _lastWarningTime = DateTime.Now;
        return samples;
    }

    public void UpdateSettings(Settings.Settings s)
    {
    }
}