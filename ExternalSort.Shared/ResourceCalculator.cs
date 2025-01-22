using System.Diagnostics.CodeAnalysis;
#if OS_WINDOWS
using System.Diagnostics;
#endif

namespace ExternalSort.Shared;

public static class ResourceCalculator
{
    private const int DefaultUsableMemoryMb = 512;
    private const float MemoryMultiplier = 0.9f;
    
    public static int GetUsableCores()
    {
        var totalCores = Environment.ProcessorCount;
        return Math.Max(totalCores - 2, 1);
    }
    
    #if OS_LINUX
    public static int GetUsableMemoryMb()
    {
        var memInfo = File.ReadAllLines("/proc/meminfo");
        var freeMemoryLine = memInfo.FirstOrDefault(line => line.StartsWith("MemFree"));

        if (string.IsNullOrEmpty(freeMemoryLine))
        {
            return DefaultUsableMemoryMb;
        }
        
        var parts = freeMemoryLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 3)
        {
            return DefaultUsableMemoryMb;
        }

        if (int.TryParse(parts[1], out var memoryKb))
        {
            var usable = (int)(memoryKb / 1024.0 * MemoryMultiplier);
            return Math.Max(usable, DefaultUsableMemoryMb);
        }

        return DefaultUsableMemoryMb;
    }
    #elif OS_WINDOWS
    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    public static int GetUsableMemoryMb()
    {
        var availableMemoryCounter = new PerformanceCounter("Memory", "Available MBytes");
        var availableMemory = availableMemoryCounter.NextValue();
        var usable = (int)(availableMemory * MemoryMultiplier);

        return Math.Max(usable, DefaultUsableMemoryMb);
    }
    #else
    public static int GetUsableMemoryMb()
    {
        return DefaultUsableMemoryMb;
    }
    #endif
}