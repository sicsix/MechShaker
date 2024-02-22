using MechShakerEngine.Interfaces;
using MechShakerReader;

namespace MechShakerEngine.Events;

public readonly record struct JumpJetsEvent(EventData EventData) : IEvent
{
    public readonly EventData EventData = EventData;
    public          bool      Active => EventData.Int0 == 1;
}