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
        Console.WriteLine("Staring sorting");
        
        var start = DateTime.Now;
        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = _parallelism
        };

        Parallel.ForEach(paths, options, SortFile);

        var spent = (DateTime.Now - start).TotalSeconds;
        Console.WriteLine($"Finished sorting in {spent}s");
    }
    
    private void SortFile(string path)
    {
        var lines = File.ReadAllLines(path);
        Array.Sort(lines, _comparer);
        
        using var writer = new StreamWriter(path, append: false);

        foreach (var line in lines)
        {
            writer.WriteLine(line);
        }

        lines = null;
        GC.Collect();
    }
}