using MechShakerEngine.Interfaces;
using MechShakerReader;

namespace MechShakerEngine.Events;

public readonly record struct MissilesEvent(EventData EventData) : IEvent
{
    public readonly EventData EventData = EventData;
    public          int       WeaponId         => EventData.Int0;
    public          float     NumberOfMissiles => EventData.Float0;
    public          float     MissileInterval  => EventData.Float1;
    public          float     Speed            => EventData.Float2;
    public          float     Impulse          => EventData.Float3;
}