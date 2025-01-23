namespace ExternalSort.CustomSorter;

public class Sorter
{
    private readonly int _parallelism;
    private readonly IComparer<string> _comparer;

    public Sorter(int parallelism, IComparer<string> comparer)
    {
        _parallelism = parallelism;
        _comparer = comparer;
    }
    
    public void SortFiles(IReadOnlyCollection<string> paths)
    {
        var start = DateTime.Now;
        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = _parallelism
        };

        Parallel.ForEach(paths, options, SortFile);
        Console.WriteLine($"Finished sorting in {(DateTime.Now - start).TotalMilliseconds}ms");
    }
    
    private void SortFile(string path)
    {
        Console.WriteLine($"Starting sort for {Path.GetFileName(path)}");
        var start = DateTime.Now;
        var lines = File.ReadAllLines(path);
        Array.Sort(lines, _comparer);
        
        using var writer = new StreamWriter(path, append: false);

        foreach (var line in lines)
        {
            writer.WriteLine(line);
        }
        
        Console.WriteLine($"Finished sorting of {Path.GetFileName(path)} in {(DateTime.Now - start).TotalMilliseconds}ms");
        Console.WriteLine($"Total memory {GC.GetTotalMemory(true) / 1024 / 1024} MB");
    }
}