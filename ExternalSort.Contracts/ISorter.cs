namespace ExternalSort.Contracts;

public interface ISorter
{
    Task SortAsync(string inputPath, string outputPath);
}