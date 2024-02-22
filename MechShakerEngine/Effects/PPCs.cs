using MechShakerEngine.Events;
using MechShakerEngine.Generators;
using NAudio.Wave;

namespace MechShakerEngine.Effects;

internal class PPCs : Effect<ProjectileEvent>
{
    private readonly List<ImpulseGenerator> _generators = new();

    private const float MaxImpulse = 1500f;

    private float _popFreq;
    private int   _popCycles;
    private int   _popAttackEnd;
    private int   _popDecayStart;
    private float _tailFreq;
    private int   _tailCycles;
    private int   _tailAttackEnd;
    private int   _tailDecayStart;
    private float _tailAmp;

    public PPCs(ISampleProvider source, Audio audio) : base(source, audio)
    {
    }

    public override void Clear()
    {
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

    protected override void OnEventDataReceived(ProjectileEvent e)
    {
        float totalImpulse  = e.Impulse        * e.NumberOfProjectiles;
        float impulseFactor = totalImpulse     / MaxImpulse;
        float popAmp        = Volume.Amplitude * impulseFactor;
        float tailAmp       = popAmp           * _tailAmp;

        _generators.Add(new ImpulseGenerator(_popFreq, popAmp, _popCycles, _popAttackEnd, _popDecayStart, Audio.Clock,
                                             canBeActiveInstance: false));
        _generators.Add(new ImpulseGenerator(_tailFreq, tailAmp, _tailCycles, _tailAttackEnd, _tailDecayStart, Audio.Clock));
    }

    protected override void OnUpdateSettings(Settings.Settings s)
    {
        UpdateVolume(s.PPCs);
        _popFreq  = s.PPCs.PopFrequency;
        _tailFreq = s.PPCs.TailFrequency;
        _tailAmp  = s.PPCs.TailAmplitude / 100f;

        _popCycles = CalculateCycles(_popFreq, s.PPCs.PopLength / 1000f);

        (_popAttackEnd, _popDecayStart) =
            CalculateAttackAndDecayCycles(_popFreq, s.PPCs.PopLength / 1000f, s.PPCs.PopAttackEnd / 100f, s.PPCs.PopDecayStart / 100f);

        _tailCycles = CalculateCycles(_tailFreq, s.PPCs.TailLength / 1000f);

        (_tailAttackEnd, _tailDecayStart) =
            CalculateAttackAndDecayCycles(_tailFreq, s.PPCs.TailLength / 1000f, s.PPCs.TailAttackEnd / 100f, s.PPCs.TailDecayStart / 100f);
    }
}