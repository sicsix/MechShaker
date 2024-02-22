using MechShakerEngine.Interfaces;
using MechShakerReader;

namespace MechShakerEngine.Events;

public readonly record struct PartDestructionEvent(EventData EventData) : IEvent
{
    public enum PartType
    {
        Head,
        CenterTorso,
        LeftTorso,
        LeftArm,
        LeftLeg,
        RightTorso,
        RightArm,
        RightLeg,
        Invalid
    }

    public readonly EventData EventData = EventData;
    public          PartType  Part => (PartType)EventData.Int0;
}