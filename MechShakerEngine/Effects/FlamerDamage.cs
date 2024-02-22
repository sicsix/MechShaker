using MechShakerEngine.Events;
using NAudio.Wave;

namespace MechShakerEngine.Effects;

internal class FlamerDamage : Effect<DamagedEvent>
{
    public FlamerDamage(ISampleProvider source, Audio audio) : base(source, audio)
    {
    }

    public override void Clear()
    {
    }

    protected override void Write(float[] buffer, int offset, int count, ref float activeInstances)
    {
    }

    protected override void OnEventDataReceived(DamagedEvent e)
    {
    }

    protected override void OnUpdateSettings(Settings.Settings s)
    {
    }
}