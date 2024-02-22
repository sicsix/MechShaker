using MechShakerEngine.Interfaces;
using MechShakerReader;

namespace MechShakerEngine.Events;

public readonly record struct MASCEvent(EventData EventData) : IEvent
{
    public readonly EventData EventData = EventData;
    public          bool      IsEngaged  => EventData.Int0 == 1;
    public          float     GaugeValue => EventData.Float0;
}