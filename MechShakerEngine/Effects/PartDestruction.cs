using MechShakerEngine.Events;
using MechShakerEngine.Generators;
using NAudio.Wave;

namespace MechShakerEngine.Effects;

internal class PartDestruction : Effect<PartDestructionEvent>
{
    private readonly List<ImpulseGenerator> _generators = new();

    private float _freq;
    private int   _cycles;
    private int   _attackEnd;
    private int   _decayStart;
    private float _secFreq;
    private float _secAmp;
    private int   _secCycles;
    private int   _secAttackEnd;
    private int   _secDecayStart;

    private float _headFactor;
    private float _centerTorsoFactor;
    private float _sideTorsoFactor;
    private float _armFactor;
    private float _legFactor;

    public PartDestruction(ISampleProvider source, Audio audio) : base(source, audio)
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

    protected override void OnEventDataReceived(PartDestructionEvent e)
    {
        Logging.At(this).Debug("Part destroyed: {Part}", e.Part);
        float factor;
        switch (e.Part)
        {
            case PartDestructionEvent.PartType.Head:
                factor = _headFactor;
                break;
            default:
            case PartDestructionEvent.PartType.Invalid:
            case PartDestructionEvent.PartType.CenterTorso:
                factor = _centerTorsoFactor;
                break;
            case PartDestructionEvent.PartType.LeftTorso:
            case PartDestructionEvent.PartType.RightTorso:
                factor = _sideTorsoFactor;
                break;
            case PartDestructionEvent.PartType.LeftArm:
            case PartDestructionEvent.PartType.RightArm:
                factor = _armFactor;
                break;
            case PartDestructionEvent.PartType.LeftLeg:
            case PartDestructionEvent.PartType.RightLeg:
                factor = _legFactor;
                break;
        }

        float amp    = Volume.Amplitude * factor;
        float secAmp = amp              * _secAmp;

        _generators.Add(new ImpulseGenerator(_freq, amp, _cycles, _attackEnd, _decayStart, Audio.Clock, canBeActiveInstance: false));
        _generators.Add(new ImpulseGenerator(_secFreq, secAmp, _secCycles, _secAttackEnd, _secDecayStart, Audio.Clock));
    }

    protected override void OnUpdateSettings(Settings.Settings s)
    {
        UpdateVolume(s.PartDestruction);
        _freq = s.PartDestruction.Frequency;
        float length     = s.PartDestruction.Length     / 1000f;
        float attackEnd  = s.PartDestruction.AttackEnd  / 100f;
        float decayStart = s.PartDestruction.DecayStart / 100f;
        _cycles                   = CalculateCycles(_freq, length);
        (_attackEnd, _decayStart) = CalculateAttackAndDecayCycles(_freq, length, attackEnd, decayStart);
        _secFreq                  = s.PartDestruction.SecondaryFrequency;
        _secAmp                   = s.PartDestruction.SecondaryAmplitude / 100f;
        float secLength     = s.PartDestruction.SecondaryLength     / 1000f;
        float secAttackEnd  = s.PartDestruction.SecondaryAttackEnd  / 100f;
        float secDecayStart = s.PartDestruction.SecondaryDecayStart / 100f;
        _secCycles                      = CalculateCycles(_secFreq, secLength);
        (_secAttackEnd, _secDecayStart) = CalculateAttackAndDecayCycles(_secFreq, secLength, secAttackEnd, secDecayStart);
        _headFactor                     = s.PartDestruction.HeadFactor        / 100f;
        _centerTorsoFactor              = s.PartDestruction.CenterTorsoFactor / 100f;
        _sideTorsoFactor                = s.PartDestruction.SideTorsoFactor   / 100f;
        _armFactor                      = s.PartDestruction.ArmFactor         / 100f;
        _legFactor                      = s.PartDestruction.LegFactor         / 100f;
    }
}