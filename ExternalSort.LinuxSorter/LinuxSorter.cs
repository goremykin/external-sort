using System.Runtime.InteropServices;
using CliWrap;
using ExternalSort.Contracts;

namespace ExternalSort.LinuxSorter;

public class LinuxSorter : ISorter
{
    private readonly int _memoryMb;
    private readonly int _parallelism;

    public LinuxSorter(int memoryMb, int parallelism)
    {
        _memoryMb = memoryMb;
        _parallelism = parallelism;
    }
    
    public async Task SortAsync(string inputPath, string outputPath)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            throw new NotSupportedException("Non linux based systems are not supported");
        }
        
        await Cli.Wrap("sort")
            .WithArguments([
                "--parallel", _parallelism.ToString(),
                "-S", $"{_memoryMb}M",
                "-t", ".",
                "-k2", "-k1n",
                inputPath
            ])
            .WithStandardOutputPipe(PipeTarget.ToFile(outputPath))
            .ExecuteAsync();
    }
}