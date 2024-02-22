using MechShakerEngine.Interfaces;
using MechShakerReader;

namespace MechShakerEngine.Events;

public readonly record struct DropshipEvent(EventData EventData) : IEvent
{
    public readonly EventData EventData = EventData;
    public          bool      TurntableStart => EventData.Int0 == 0;
    public          bool      TurntableEnd   => EventData.Int0 == 1;
    public          bool      Flight         => EventData.Int0 == 2;
    public          bool      Landed         => EventData.Int0 == 3;
}