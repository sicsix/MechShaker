using System.Runtime.InteropServices;

namespace MechShakerReader;

[StructLayout(LayoutKind.Sequential)]
public readonly struct ControlBlock
{
    public readonly long WriteIndex;
    public readonly long PacketNumber;
}