using System.Runtime.InteropServices;

namespace MechShakerReader;

[StructLayout(LayoutKind.Sequential)]
public readonly record struct EventData(
    int   EventCode,
    int   Int0,
    float Float0,
    float Float1,
    float Float2,
    float Float3,
    float Float4,
    float Float5);