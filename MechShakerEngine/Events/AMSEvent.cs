using MechShakerEngine.Interfaces;
using MechShakerReader;

namespace MechShakerEngine.Events;

public readonly record struct AMSEvent(EventData EventData) : IEvent
{
    public readonly EventData EventData = EventData;
    public          int       WeaponId   => EventData.Int0;
    public          float     RateOfFire => EventData.Float0;
    public          bool      Active     => EventData.Float5 == 1f;
}