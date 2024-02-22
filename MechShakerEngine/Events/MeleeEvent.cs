using MechShakerEngine.Interfaces;
using MechShakerReader;

namespace MechShakerEngine.Events;

public readonly record struct MeleeEvent(EventData EventData) : IEvent
{
    public readonly EventData EventData = EventData;
    public          int       MassInTons => EventData.Int0;
    public          bool      IsHit      => EventData.Float0 == 1;
}