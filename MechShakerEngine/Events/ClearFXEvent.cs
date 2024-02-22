using MechShakerEngine.Interfaces;
using MechShakerReader;

namespace MechShakerEngine.Events;

public readonly record struct ClearFXEvent(EventData EventData) : IEvent
{
    public readonly EventData EventData = EventData;
}