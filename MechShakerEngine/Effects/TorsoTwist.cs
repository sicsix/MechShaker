using MechShakerEngine.Events;
using MechShakerEngine.Generators;
using NAudio.Wave;

namespace MechShakerEngine.Effects;

internal class TorsoTwist : Effect<TorsoTwistEvent>
{
    private readonly WaveGenerator _waveGenerator = new(false);

    private float _freq;
    private float _transitionTime;
    private float _massFactor;

    private TorsoTwistEvent _prevEvt;

    public TorsoTwist(ISampleProvider source, Audio audio) : base(source, audio)
    {
    }

    public override void Clear()
    {
        _waveGenerator.SetTarget(_freq, 0, _transitionTime);
    }

    protected override void Write(float[] buffer, int offset, int count, ref float activeInstances)
    {
        _waveGenerator.Write(buffer, offset, count, ref activeInstances);
    }

    protected override void OnEventDataReceived(TorsoTwistEvent e)
    {
        float rocPitch = MathF.Abs((e.Pitch - _prevEvt.Pitch) / e.DeltaSeconds);
        float rocYaw   = MathF.Abs((e.Yaw   - _prevEvt.Yaw)   / e.DeltaSeconds);

        float rocPitchPct = rocPitch / e.MaxPitchRate;
        float rocYawPct   = rocYaw   / e.MaxYawRate;
        float maxPct      = Math.Clamp(MathF.Max(rocPitchPct, rocYawPct), 0, 1);

        float massRatio     = Math.Clamp((e.MassInTons - 20f) / 80, 0f, 1f);
        float massMinAmp    = _massFactor;
        float massMaxAmp    = 1.0f;
        float massAmpEffect = Lerp(massMinAmp, massMaxAmp, massRatio);

        float amplitude = Volume.Amplitude * maxPct * massAmpEffect;
        _waveGenerator.SetTarget(_freq, amplitude, _transitionTime);

        _prevEvt = e;
    }

    protected override void OnUpdateSettings(Settings.Settings s)
    {
        UpdateVolume(s.TorsoTwist);
        _freq           = s.TorsoTwist.Frequency;
        _transitionTime = s.TorsoTwist.TransitionTime / 1000f;
        _massFactor     = s.TorsoTwist.MassFactor     / 100f;
    }
}