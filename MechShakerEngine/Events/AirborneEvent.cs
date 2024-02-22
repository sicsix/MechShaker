using MechShakerEngine.Interfaces;
using MechShakerReader;

namespace MechShakerEngine.Events;

public readonly record struct AirborneEvent(EventData EventData) : IEvent
{
    public readonly EventData EventData = EventData;
    public          bool      Airborne => EventData.Int0 == 1;
}