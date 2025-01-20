using System.Runtime.InteropServices;
using CliWrap;

namespace ExternalSort.Sorter;

public class LinuxSorter
{
    public async Task SortAsync(string inputPath, string outputPath)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            throw new NotSupportedException("Non linux systems are not supported");
        }
        
        var availableMemory = GetAvailableMemoryKb();
        var useMemoryMb = availableMemory.HasValue ? (int)(availableMemory / 1024.0 * 0.8) : 128;
        var useCores = Math.Max(Environment.ProcessorCount - 2, 1);
        
        await Cli.Wrap("sort")
            .WithArguments([
                "--parallel", useCores.ToString(),
                "-S", $"{useMemoryMb}M",
                "-t", ".",
                "-k2", "-k1n",
                inputPath
            ])
            .WithStandardOutputPipe(PipeTarget.ToFile(outputPath))
            .ExecuteAsync();
    }

    private int? GetAvailableMemoryKb()
    {
        var memInfo = File.ReadAllLines("/proc/meminfo");
        var freeMemoryLine = memInfo.FirstOrDefault(line => line.StartsWith("MemFree"));

        if (string.IsNullOrEmpty(freeMemoryLine))
        {
            return null;
        }

        var parts = freeMemoryLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 3)
        {
            return null;
        }

        return int.TryParse(parts[1], out var memory) ? memory : null;
    }
}