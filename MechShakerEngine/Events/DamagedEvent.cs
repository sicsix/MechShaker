using MechShakerEngine.Interfaces;
using MechShakerReader;

namespace MechShakerEngine.Events;

public readonly record struct DamagedEvent(EventData EventData) : IEvent
{
    public enum DamageType
    {
        Trace,
        Projectile,
        Missile,
        Melee,
        Explosion
    }

    public readonly EventData  EventData = EventData;
    public          DamageType Type         => (DamageType)EventData.Int0;
    public          float      Damage       => EventData.Float0;
    public          float      RateOfFire   => EventData.Float1;
    public          float      Duration     => EventData.Float2;
    public          int        InstigatorID => (int)EventData.Float5;
    public          bool       IsMG         => Duration == 0 && RateOfFire != 0;
    public          bool       IsFlamer     => Duration == 0 && RateOfFire == 0;
}