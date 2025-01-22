namespace ExternalSort.CustomSorter;

public class Sorter
{
    private readonly int _parallelism;
    
    public Sorter(int parallelism)
    {
        _parallelism = parallelism;
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
        var lines = File.ReadAllLines(path);
        Array.Sort(lines);
        
        using var stream = File.Open(path, FileMode.Create);
        using var writer = new StreamWriter(stream);

        foreach (var line in lines)
        {
            writer.WriteLine(line);
        }
    }
}