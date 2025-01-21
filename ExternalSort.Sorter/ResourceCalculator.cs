#if OS_WINDOWS
using System.Diagnostics;
#endif

namespace ExternalSort.Sorter;

public static class ResourceCalculator
{
    private const int DefaultUsableMemoryMb = 512;
    
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
            return (int)(memoryKb / 1024.0 * 0.8);
        }

        return DefaultUsableMemoryMb;
    }
    #elif OS_WINDOWS
    public static int GetUsableMemoryMb()
    {
        var availableMemoryCounter = new PerformanceCounter("Memory", "Available MBytes");
        var availableMemory = availableMemoryCounter.NextValue();

        return (int)(availableMemory * 0.8);
    }
    #else
    public static int GetUsableMemoryMb()
    {
        return DefaultUsableMemoryMb;
    }
    #endif
}