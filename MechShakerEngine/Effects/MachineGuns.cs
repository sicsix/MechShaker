using MechShakerEngine.Events;
using MechShakerEngine.Generators;
using NAudio.Wave;

namespace MechShakerEngine.Effects;

internal class MachineGuns : Effect<TraceEvent>
{
    private record struct ActiveMachineGun(int WeaponId, uint CycleRate)
    {
        public readonly int  WeaponId       = WeaponId;
        public readonly uint CycleRate      = CycleRate;
        public          uint LastClockFired = 0;
    }

    private readonly List<ActiveMachineGun> _active = new();

    private readonly Dictionary<int, List<ImpulseGenerator>> _generators = new();

    private float _freq;
    private int   _cycles;
    private int   _attackEnd;
    private int   _decayStart;

    public MachineGuns(ISampleProvider source, Audio audio) : base(source, audio)
    {
    }

    public override void Clear()
    {
        _active.Clear();
        foreach ((int _, var generators) in _generators)
        {
            generators.Clear();
        }

        _generators.Clear();
    }

    protected override void Write(float[] buffer, int offset, int count, ref float activeInstances)
    {
        float amp = Volume.Amplitude;

        for (int i = 0; i < _active.Count; i++)
        {
            var active = _active[i];

            if (!_generators.TryGetValue(active.WeaponId, out var generators))
                _generators.Add(active.WeaponId, generators = new List<ImpulseGenerator>());

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

        foreach ((int _, var generators) in _generators)
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

    protected override void OnEventDataReceived(TraceEvent e)
    {
        if (e.Active)
        {
            _active.RemoveAll(o => o.WeaponId == e.WeaponId);
            uint cycleRate = (uint)(1f / e.RateOfFire * Audio.SampleRate);
            _active.Add(new ActiveMachineGun(e.WeaponId, cycleRate));
        }
        else
            _active.RemoveAll(o => o.WeaponId == e.WeaponId);
    }

    protected override void OnUpdateSettings(Settings.Settings s)
    {
        UpdateVolume(s.MachineGuns);
        _freq = s.MachineGuns.Frequency;
        float length     = s.MachineGuns.Length     / 1000f;
        float attackEnd  = s.MachineGuns.AttackEnd  / 100f;
        float decayStart = s.MachineGuns.DecayStart / 100f;
        _cycles                   = CalculateCycles(_freq, length);
        (_attackEnd, _decayStart) = CalculateAttackAndDecayCycles(_freq, length, attackEnd, decayStart);
    }
}