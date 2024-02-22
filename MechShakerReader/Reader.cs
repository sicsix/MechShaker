using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;

namespace MechShakerReader;

public static class Reader
{
    private const int    BufferSize           = 128;
    private const int    ControlBlockSize     = 16;
    private const int    EventDataSize        = 32;
    private const string MemoryMappedFileName = "MechShakerBridgeMemory";


    public static async IAsyncEnumerable<EventData> Read([EnumeratorCancellation] CancellationToken token)
    {
        MemoryMappedFile?         mmf          = null;
        MemoryMappedViewAccessor? accessor     = null;
        var                       controlBlock = new ControlBlock();
        var                       eventList    = new List<EventData>();

        while (!token.IsCancellationRequested)
        {
            try
            {
                if (mmf == null || accessor == null)
                {
                    try
                    {
                        mmf      = MemoryMappedFile.OpenExisting(MemoryMappedFileName);
                        accessor = mmf.CreateViewAccessor();
                        Logging.At(typeof(Reader)).Information("Connected to MechShakerBridge");
                        Thread.MemoryBarrier();
                        accessor.Read(0, out controlBlock);
                        Thread.MemoryBarrier();
                    }
                    catch (FileNotFoundException)
                    {
                        Logging.At(typeof(Reader)).Information("Waiting for connection to MechShakerBridge...");
                        await Task.Delay(5000, token);
                        continue;
                    }
                }

                var controlBlockBefore = controlBlock;
                Thread.MemoryBarrier();
                accessor.Read(0, out controlBlock);
                Thread.MemoryBarrier();

                long readIndex = controlBlockBefore.WriteIndex;
                for (long packetNumber = controlBlockBefore.PacketNumber; packetNumber < controlBlock.PacketNumber; packetNumber++)
                {
                    accessor.Read(ControlBlockSize + EventDataSize * readIndex, out EventData eventData);
                    readIndex = (readIndex         + 1) % BufferSize;

                    eventList.Add(eventData);
                }
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or TaskCanceledException)
            {
                CleanUpOnConnectionLoss(ref mmf, ref accessor);
            }

            for (int i = 0; i < eventList.Count; i++)
            {
                var eventData = eventList[i];

                yield return eventData;

                if (eventData.EventCode != -1)
                    continue;

                CleanUpOnConnectionLoss(ref mmf, ref accessor);
                eventList.Clear();
            }

            eventList.Clear();

            try
            {
                await Task.Delay(3, token);
            }
            catch (TaskCanceledException)
            {
                CleanUpOnConnectionLoss(ref mmf, ref accessor);
            }
        }
    }

    private static void CleanUpOnConnectionLoss(ref MemoryMappedFile? mmf, ref MemoryMappedViewAccessor? accessor)
    {
        Logging.At(typeof(Reader)).Information("Lost connection to MechShakerBridge");
        mmf?.Dispose();
        mmf = null;
        accessor?.Dispose();
        accessor = null;
    }
}