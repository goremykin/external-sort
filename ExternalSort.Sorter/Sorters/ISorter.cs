namespace ExternalSort.Sorter.Sorters;

public interface ISorter
{
    Task SortAsync(string inputPath, string outputPath);
}