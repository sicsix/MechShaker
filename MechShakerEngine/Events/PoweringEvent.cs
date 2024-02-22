using MechShakerEngine.Interfaces;
using MechShakerReader;

namespace MechShakerEngine.Events;

public readonly record struct PoweringEvent(EventData EventData) : IEvent
{
    public readonly EventData EventData = EventData;
    public          bool      Initialising => EventData.Int0 == 0;
    public          bool      PoweringUp   => EventData.Int0 == 1;
    public          bool      ShuttingDown => EventData.Int0 == 2;
}