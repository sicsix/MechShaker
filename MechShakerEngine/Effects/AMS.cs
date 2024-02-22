using System.Runtime.CompilerServices;
using MechShakerEngine.Events;
using MechShakerEngine.Generators;
using NAudio.Wave;

namespace MechShakerEngine.Effects;

internal class AMS : Effect<AMSEvent>
{
    private record struct ActiveAMS(int WeaponId, uint CycleRate)
    {
        public readonly int  WeaponId       = WeaponId;
        public readonly uint CycleRate      = CycleRate;
        public          uint LastClockFired = 0;
    }

    private readonly List<ActiveAMS>                         _active         = new();
    private readonly Dictionary<int, List<ImpulseGenerator>> _generatorsDict = new();

    private float _freq;
    private int   _cycles;
    private int   _attackEnd;
    private int   _decayStart;

    public AMS(ISampleProvider source, Audio audio) : base(source, audio)
    {
    }

    public override void Clear()
    {
        _active.Clear();
        foreach ((int _, var generators) in _generatorsDict)
        {
            generators.Clear();
        }

        _generatorsDict.Clear();
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    protected override void Write(float[] buffer, int offset, int count, ref float activeInstances)
    {
        float amp = Volume.Amplitude;

        for (int i = 0; i < _active.Count; i++)
        {
            var active = _active[i];

            if (!_generatorsDict.TryGetValue(active.WeaponId, out var generators))
                _generatorsDict.Add(active.WeaponId, generators = new List<ImpulseGenerator>());

            if (active.LastClockFired == 0)
            {
                generators.Add(new ImpulseGenerator(_freq, amp, _cycles, _attackEnd, _decayStart, Audio.Clock, canBeActiveInstance: false));
                active.LastClockFired = Audio.Clock;
            }

            uint bufferEnd = Audio.Clock           + (uint)count;
            uint fireOn    = active.LastClockFired + active.CycleRate;

            while (fireOn <= bufferEnd)
            {
                generators.Add(new ImpulseGenerator(_freq, amp, _cycles, _attackEnd, _decayStart, Audio.Clock, fireOn, false));
                active.LastClockFired =  fireOn;
                fireOn                += active.CycleRate;
            }

            _active[i] = active;
        }

        foreach ((int _, var generators) in _generatorsDict)
        {
            if (generators.Count > 0)
                activeInstances += 1;

            for (int i = 0; i < generators.Count; i++)
            {
                if (generators[i].Complete)
                {
                    generators.RemoveAt(i--);
                    continue;
                }

                generators[i].Write(buffer, offset, count, ref activeInstances);
            }
        }
    }

    protected override void OnEventDataReceived(AMSEvent e)
    {
        if (e.Active)
        {
            _active.RemoveAll(o => o.WeaponId == e.WeaponId);
            uint cycleRate = (uint)(1f / e.RateOfFire * Audio.SampleRate);
            _active.Add(new ActiveAMS(e.WeaponId, cycleRate));
        }
        else
            _active.RemoveAll(o => o.WeaponId == e.WeaponId);
    }

    protected override void OnUpdateSettings(Settings.Settings s)
    {
        UpdateVolume(s.AMS);
        _freq = s.AMS.Frequency;
        float length     = s.AMS.Length     / 1000f;
        float attackEnd  = s.AMS.AttackEnd  / 100f;
        float decayStart = s.AMS.DecayStart / 100f;
        _cycles                   = CalculateCycles(_freq, length);
        (_attackEnd, _decayStart) = CalculateAttackAndDecayCycles(_freq, length, attackEnd, decayStart);
    }
}