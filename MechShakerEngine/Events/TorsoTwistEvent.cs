using MechShakerEngine.Interfaces;
using MechShakerReader;

namespace MechShakerEngine.Events;

public readonly record struct TorsoTwistEvent(EventData EventData) : IEvent
{
    public readonly EventData EventData = EventData;
    public          int       MassInTons    => EventData.Int0;
    public          float     DeltaSeconds  => EventData.Float0;
    public          float     VelocityInKph => EventData.Float1;
    public          float     MaxYawRate    => EventData.Float2;
    public          float     MaxPitchRate  => EventData.Float3;
    public          float     Yaw           => EventData.Float4;
    public          float     Pitch         => EventData.Float5;
}