using ExternalSort.Contracts;
using ExternalSort.Shared;

namespace ExternalSort.CustomSorter;

public class CustomSorter : ISorter
{
    public async Task SortAsync(string inputPath, string outputPath)
    {
        var inputFile = new FileInfo(inputPath);
        var usableMemoryMb = ResourceCalculator.GetUsableMemoryMb();
        var realisticMemoryBytes = (long)(usableMemoryMb * .75) * 1024 * 1024;

        if (inputFile.Length <= realisticMemoryBytes)
        {
            // todo: sort in memory
        }

        var chunkSize = inputFile.Length / realisticMemoryBytes; // todo: adjust
        var tempDir = Directory.CreateTempSubdirectory().FullName;
        var splitter = new Splitter();
        var tempFiles = splitter.Split(inputPath, tempDir, chunkSize);

        Directory.Delete(tempDir);
    }
}