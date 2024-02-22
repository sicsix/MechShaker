﻿using MathNet.Filtering;
using MechShakerEngine.Interfaces;
using NAudio.Wave;

namespace MechShakerEngine.Processors;

internal class LowPassFilter : ISampleProvider, ISampler
{
    public WaveFormat WaveFormat { get; }

    private          bool         _enabled;
    private          float        _cuttOffFreq;
    private readonly int          _order;
    private          OnlineFilter _filter;

    private readonly ISampleProvider _source;

    public LowPassFilter(ISampleProvider source, int cutOffFreq, int order)
    {
        WaveFormat   = source.WaveFormat;
        _cuttOffFreq = cutOffFreq;
        _order       = order;
        _source      = source;
        _filter      = OnlineFilter.CreateLowpass(ImpulseResponse.Finite, source.WaveFormat.SampleRate, cutOffFreq, order);
    }

    public void Clear()
    {
    }

    public int Read(float[] buffer, int offset, int count)
    {
        int samples = _source.Read(buffer, offset, count);

        if (!_enabled)
            return samples;

        for (int i = 0; i < samples; i++)
        {
            buffer[offset + i] = (float)_filter.ProcessSample(buffer[offset + i]);
        }

        return samples;
    }

    public void UpdateSettings(Settings.Settings s)
    {
        bool switchedState = s.LowPassFilter.Enabled != _enabled;
        _enabled = s.LowPassFilter.Enabled;

        if (_cuttOffFreq == s.LowPassFilter.Frequency && !switchedState)
            return;

        Logging.At(this).Debug("{State}: {Hz}Hz", _enabled ? "Enabled" : "Disabled", s.LowPassFilter.Frequency);
        _cuttOffFreq = s.LowPassFilter.Frequency;
        _filter      = OnlineFilter.CreateLowpass(ImpulseResponse.Finite, WaveFormat.SampleRate, _cuttOffFreq, _order);
    }
}