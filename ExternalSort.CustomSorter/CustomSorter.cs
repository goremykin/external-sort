using System.Diagnostics;
using ExternalSort.Contracts;
using ExternalSort.Shared;

namespace ExternalSort.CustomSorter;

public class CustomSorter : ISorter
{
    private const int AverageLineSize = 32 * 2 + 20;
    
    public Task SortAsync(string inputPath, string outputPath)
    {
        var usableCores = ResourceCalculator.GetUsableCores();
        var usableMemoryMb = ResourceCalculator.GetUsableMemoryMb();
        Console.WriteLine($"Usable Memory: {usableMemoryMb}");
        var realisticMemoryBytes = (long)(usableMemoryMb * .75) * 1024 * 1024;
        Console.WriteLine($"Realistic Memory: {realisticMemoryBytes / 1024 / 1024} MB");
        var inputFile = new FileInfo(inputPath);
        var maxLinesFitMemory = realisticMemoryBytes / AverageLineSize;
        var approximateFileLines = inputFile.Length / AverageLineSize;
        var linesPerChunk = Math.Min(maxLinesFitMemory, approximateFileLines) / usableCores;
        
        var tempDir = Directory.CreateTempSubdirectory().FullName;
        var tempFiles = Splitter.Split(inputPath, tempDir, linesPerChunk);
        var sorter = new Sorter(usableCores);

        sorter.SortFiles(tempFiles);
        
        Directory.Delete(tempDir, recursive: true);
        MemoryUsage();
        
        return Task.CompletedTask;
    }
    
    void MemoryUsage()
    {
        Console.WriteLine(
            $"{Process.GetCurrentProcess().PeakWorkingSet64 / 1024 / 1024} MB peak working set | {Process.GetCurrentProcess().PrivateMemorySize64 / 1024 / 1024} MB private bytes");
    }
}