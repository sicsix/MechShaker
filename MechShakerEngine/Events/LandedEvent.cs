using MechShakerEngine.Interfaces;
using MechShakerReader;

namespace MechShakerEngine.Events;

public readonly record struct LandedEvent(EventData EventData) : IEvent
{
    public readonly EventData EventData = EventData;
    public          int       MassInTons         => EventData.Int0;
    public          float     AccelerationInKmh2 => EventData.Float0;
}