using System.Runtime.CompilerServices;
using Serilog;

namespace MechShakerReader;

internal static class Logging
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ILogger At(object caller)
    {
        return At(caller.GetType());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ILogger At(string name)
    {
        return Log.ForContext("Class", $"[{name}]");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ILogger At(Type type)
    {
        return Log.ForContext("Class", $"[{type.Name}]");
    }
}