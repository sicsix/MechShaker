using MechShakerEngine.Events;
using MechShakerEngine.Generators;
using MechShakerEngine.Interfaces;
using NAudio.Wave;

namespace MechShakerEngine.Effects;

internal class Missiles : Effect<MissilesEvent>
{
    private readonly List<IGenerator> _generators = new();

    private const float MaxImpulse  = 12000f;
    private const float SpeedFactor = 20000f;

    private float _launchFreq;
    private int   _launchCycles;
    private int   _launchAttackEnd;
    private int   _launchDecayStart;
    private float _tailFreq;
    private int   _tailCycles;
    private float _totalLength;
    private int   _tailAttackEnd;
    private int   _tailDecayStart;
    private float _tailAmp;
    private float _streamFactor;

    public Missiles(ISampleProvider source, Audio audio) : base(source, audio)
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

    protected override void OnEventDataReceived(MissilesEvent e)
    {
        float totalImpulse  = e.MissileInterval == 0 ? e.Impulse * e.NumberOfMissiles : e.Impulse;
        float impulseFactor = MathF.Sqrt(totalImpulse / MaxImpulse * e.Speed / SpeedFactor);
        float launchAmp     = Volume.Amplitude * impulseFactor;
        float tailAmp       = launchAmp        * _tailAmp;

        if (e.MissileInterval == 0)
        {
            _generators.Add(new ImpulseGenerator(_launchFreq, launchAmp, _launchCycles, _launchAttackEnd, _launchDecayStart, Audio.Clock,
                                                 canBeActiveInstance: false));
            _generators.Add(new ImpulseGenerator(_tailFreq, tailAmp, _tailCycles, _tailAttackEnd, _tailDecayStart, Audio.Clock));
            return;
        }

        launchAmp *= _streamFactor;
        tailAmp   *= _streamFactor;

        _generators.Add(new DummyGenerator(_totalLength + e.MissileInterval * (e.NumberOfMissiles - 1)));

        for (int i = 0; i < e.NumberOfMissiles; i++)
        {
            float delay  = i * e.MissileInterval;
            uint  fireAt = (uint)(delay * Audio.SampleRate + Audio.Clock);
            _generators.Add(new ImpulseGenerator(_launchFreq, launchAmp, _launchCycles, _launchAttackEnd, _launchDecayStart, Audio.Clock,
                                                 fireAt, false));
            _generators.Add(new ImpulseGenerator(_tailFreq, tailAmp, _tailCycles, _tailAttackEnd, _tailDecayStart, Audio.Clock, fireAt,
                                                 false));
        }
    }

    protected override void OnUpdateSettings(Settings.Settings s)
    {
        UpdateVolume(s.Missiles);
        _launchFreq   = s.Missiles.LaunchFrequency;
        _tailFreq     = s.Missiles.TailFrequency;
        _tailAmp      = s.Missiles.TailAmplitude / 100f;
        _streamFactor = s.Missiles.StreamFactor  / 100f;

        _launchCycles = CalculateCycles(_launchFreq, s.Missiles.LaunchLength / 1000f);

        (_launchAttackEnd, _launchDecayStart) = CalculateAttackAndDecayCycles(_launchFreq, s.Missiles.LaunchLength / 1000f,
                                                                              s.Missiles.LaunchAttackEnd           / 100f,
                                                                              s.Missiles.LaunchDecayStart          / 100f);

        _totalLength = Math.Max(s.Missiles.LaunchLength, s.Missiles.TailLength) / 1000f;
        _tailCycles  = CalculateCycles(_tailFreq, s.Missiles.TailLength / 1000f);

        (_tailAttackEnd, _tailDecayStart) = CalculateAttackAndDecayCycles(_tailFreq, s.Missiles.TailLength / 1000f,
                                                                          s.Missiles.TailAttackEnd         / 100f,
                                                                          s.Missiles.TailDecayStart        / 100f);
    }
}