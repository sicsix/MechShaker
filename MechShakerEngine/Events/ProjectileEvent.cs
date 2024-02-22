using MechShakerEngine.Interfaces;
using MechShakerReader;

namespace MechShakerEngine.Events;

public readonly record struct ProjectileEvent(EventData EventData) : IEvent
{
    public readonly EventData EventData = EventData;
    public          int       WeaponId            => EventData.Int0;
    public          int       NumberOfTimesToFire => (int)EventData.Float0;
    public          float     DelayBetweenFiring  => EventData.Float1;
    public          int       NumberOfProjectiles => (int)EventData.Float2;
    public          float     Speed               => EventData.Float3;
    public          bool      IsPPC               => EventData.Float4 == 0;
    public          float     Impulse             => EventData.Float5;
}