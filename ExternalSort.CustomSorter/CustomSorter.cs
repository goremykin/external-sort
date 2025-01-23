using System.Diagnostics;
using ExternalSort.Contracts;
using ExternalSort.Shared;

namespace ExternalSort.CustomSorter;

public class CustomSorter : ISorter
{
    private const int AverageLineLength = 27;
    private const int AverageLineSizeInMem = AverageLineLength * 3; // very roughly
    
    public Task SortAsync(string inputPath, string outputPath)
    {
        var usableCores = ResourceCalculator.GetUsableCores();
        var usableMemoryMb = ResourceCalculator.GetUsableMemoryMb();
        Console.WriteLine($"Usable Memory: {usableMemoryMb}");
        var realisticMemoryBytes = (long)(usableMemoryMb * .75) * 1024 * 1024;
        Console.WriteLine($"Realistic Memory: {realisticMemoryBytes / 1024 / 1024} MB");
        var inputFile = new FileInfo(inputPath);
        var maxLinesFitMemory = realisticMemoryBytes / AverageLineSizeInMem;
        var approximateFileLines = inputFile.Length / AverageLineLength;
        var linesPerChunk = Math.Min(maxLinesFitMemory, approximateFileLines) / usableCores;
        
        var tempDir = Directory.CreateTempSubdirectory().FullName;
        var tempFiles = Splitter.Split(inputPath, tempDir, linesPerChunk);
        Console.WriteLine($"Number of chunks: {tempFiles.Count}");
        var comparer = new LineComparer();
        var sorter = new Sorter(usableCores, comparer);
        var merger = new Merger(comparer);

        sorter.SortFiles(tempFiles);
        merger.Merge(tempFiles, outputPath);
        
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