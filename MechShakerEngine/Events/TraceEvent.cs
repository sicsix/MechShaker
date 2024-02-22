using MechShakerEngine.Interfaces;
using MechShakerReader;

namespace MechShakerEngine.Events;

public readonly record struct TraceEvent(EventData EventData) : IEvent
{
    public readonly EventData EventData = EventData;
    public          int       WeaponId               => EventData.Int0;
    public          float     DamageOverDuration     => EventData.Float0;
    public          float     HeatDamageOverDuration => EventData.Float1;
    public          float     RateOfFire             => EventData.Float2;
    public          float     Duration               => EventData.Float3;
    public          bool      Active                 => EventData.Float5 == 1f;
    public          bool      IsTAG                  => Duration == -1 && DamageOverDuration == 0;
    public          bool      IsMG                   => Duration == 0  && RateOfFire         != 0;
    public          bool      IsAMS                  => Duration == -1 && DamageOverDuration > 0;
    public          bool      IsFlamer               => Duration == 0  && RateOfFire         == 0;
}