using MechShakerEngine.Interfaces;
using MechShakerReader;

namespace MechShakerEngine.Events;

public readonly record struct BridgeClosedEvent(EventData EventData) : IEvent
{
    public readonly EventData EventData = EventData;
}