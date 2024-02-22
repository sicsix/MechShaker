using MechShakerEngine.Interfaces;
using MechShakerEngine.Settings;
using NAudio.Wave;

namespace MechShakerEngine;

internal abstract class Effect<T> : ISampleProvider, ISampler, IEffect where T : IEvent
{
    public bool       Enabled    { get; set; }
    public Volume     Volume     { get; private set; }
    public WaveFormat WaveFormat { get; }

    protected readonly Audio           Audio;
    private readonly   ISampleProvider _source;
    private readonly   object          _lock = new();

    private float _activeInstances;

    public float GetActiveInstances()
    {
        return _activeInstances;
    }

    protected Effect(ISampleProvider source, Audio audio)
    {
        WaveFormat = source.WaveFormat;
        Audio      = audio;
        _source    = source;
    }

    protected void UpdateVolume(VolumeSettings s)
    {
        bool  switchedState = s.Enabled != Enabled;
        float previousdB    = Volume.dB;

        Enabled = s.Enabled;
        SetDecibelsFromMaster(s.Volume);

        if (switchedState || previousdB != Volume.dB)
        {
            Logging.At(this).Debug("{State}: {Rel}dB relative ({Vol}dB, {Amp:f3} amp)", Enabled ? "Enabled" : "Disabled", s.Volume,
                                   Volume.dB, Volume.Amplitude);
        }
    }

    public abstract void Clear();

    public void SetDecibelsFromMaster(float db)
    {
        Volume = new Volume(Audio.MasterVolume, db);
    }

    private protected float GetAmplitude(float db)
    {
        return GetVolume(db).Amplitude;
    }

    private protected Volume GetVolume(float db)
    {
        return new Volume(Volume, db);
    }

    public int Read(float[] buffer, int offset, int count)
    {
        _activeInstances = 0;
        int samples = _source.Read(buffer, offset, count);

        if (!Enabled)
            return samples;

        lock (_lock)
        {
            Write(buffer, offset, count, ref _activeInstances);
        }

        return samples;
    }

    public void WriteToGroup(float[] buffer, int offset, int count)
    {
        _activeInstances = 0;

        if (!Enabled)
            return;

        lock (_lock)
        {
            Write(buffer, offset, count, ref _activeInstances);
        }
    }

    protected abstract void Write(float[] buffer, int offset, int count, ref float activeInstances);

    public void EventDataReceived(T e)
    {
        lock (_lock)
        {
            OnEventDataReceived(e);
        }
    }

    protected abstract void OnEventDataReceived(T e);

    public void UpdateSettings(Settings.Settings s)
    {
        lock (_lock)
        {
            OnUpdateSettings(s);
        }
    }

    protected abstract void OnUpdateSettings(Settings.Settings s);

    // Attenuation is in decibels/m
    private const float AttenLow     = 0.05f;
    private const float AttenHigh    = 0.3f;
    private const float AttenStartHz = 10f;
    private const float AttenEndHz   = 120f;

    protected static float Attenuate(float freq, float amp, float distance)
    {
        float clampedFreq = Math.Max(Math.Min(freq, AttenEndHz), AttenStartHz) - AttenStartHz;
        float coefficient = AttenLow + (AttenHigh - AttenLow) * (clampedFreq / (AttenEndHz - AttenStartHz));
        float attenuation = MathF.Exp(-coefficient * distance);
        float amplitude   = attenuation * amp;
        return amplitude;
    }

    protected static float Lerp(float start, float end, float input)
    {
        return start * (1 - input) + end * input;
    }

    protected static int CalculateCycles(float frequency, float length)
    {
        return Math.Max((int)(length / (1 / frequency)), 2);
    }

    protected static (int attackEnd, int decayStart) CalculateAttackAndDecayCycles(float frequency,
                                                                                   float length,
                                                                                   float attackEnd,
                                                                                   float decayStart)
    {
        int totalCycles     = CalculateCycles(frequency, length);
        int attackEndCycle  = Math.Max((int)(attackEnd  * totalCycles), 1);
        int decayStartCycle = Math.Min((int)(decayStart * totalCycles), totalCycles - 1);
        decayStartCycle = Math.Max(attackEndCycle, decayStartCycle);
        return (attackEndCycle, decayStartCycle);
    }
}