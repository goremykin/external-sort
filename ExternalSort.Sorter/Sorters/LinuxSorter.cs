using System.Runtime.InteropServices;
using CliWrap;

namespace ExternalSort.Sorter.Sorters;

public class LinuxSorter : ISorter
{
    public async Task SortAsync(string inputPath, string outputPath)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            throw new NotSupportedException("Non linux based systems are not supported");
        }
        
        var usableMemoryMb = ResourceCalculator.GetUsableMemoryMb();
        var usableCores = ResourceCalculator.GetUsableCores();
        
        await Cli.Wrap("sort")
            .WithArguments([
                "--parallel", usableCores.ToString(),
                "-S", $"{usableMemoryMb}M",
                "-t", ".",
                "-k2", "-k1n",
                inputPath
            ])
            .WithStandardOutputPipe(PipeTarget.ToFile(outputPath))
            .ExecuteAsync();
    }
}