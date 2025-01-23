using ExternalSort.Contracts;

namespace ExternalSort.CustomSorter;

public class CustomSorter : ISorter
{
    private const int AverageLineLength = 27;
    private const int AverageLineSizeInMem = AverageLineLength * 3; // very roughly
    
    private readonly int _memoryMb;
    private readonly int _parallelism;

    public CustomSorter(int memoryMb, int parallelism)
    {
        _memoryMb = memoryMb;
        _parallelism = parallelism;
    }
    
    public Task SortAsync(string inputPath, string outputPath)
    {
        var realisticMemoryBytes = (long)(_memoryMb * .8) * 1024 * 1024;
        var inputFile = new FileInfo(inputPath);
        var maxLinesFitMemory = realisticMemoryBytes / AverageLineSizeInMem;
        var approximateFileLines = inputFile.Length / AverageLineLength;
        var linesPerChunk = Math.Min(maxLinesFitMemory, approximateFileLines) / _parallelism;
        
        var tempDir = Directory.CreateTempSubdirectory().FullName;
        Console.WriteLine($"Temp directory: {tempDir}");
        var tempFiles = Splitter.Split(inputPath, tempDir, linesPerChunk);
        var comparer = new LineComparer();
        var sorter = new Sorter(_parallelism, comparer);
        var merger = new Merger(comparer);

        sorter.SortFiles(tempFiles);
        merger.Merge(tempFiles, outputPath);
        
        Directory.Delete(tempDir, recursive: true);
        
        return Task.CompletedTask;
    }
}