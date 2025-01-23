namespace ExternalSort.CustomSorter;

public class Merger
{
    private readonly IComparer<string> _comparer;

    public Merger(IComparer<string> comparer)
    {
        _comparer = comparer;
    }

    public void Merge(IReadOnlyCollection<string> paths, string outputPath)
    {
        Console.WriteLine("Starting merging");
        
        var start = DateTime.Now;
        var queue = new PriorityQueue<StreamReader, string>(_comparer);

        try
        {
            foreach (var path in paths)
            {
                var reader = new StreamReader(path);
                var line = reader.ReadLine();

                if (line != null)
                {
                    queue.Enqueue(reader, line);
                }
            }

            using var writer = new StreamWriter(outputPath, append: false);

            while (queue.TryDequeue(out var reader, out var line))
            {
                writer.WriteLine(line);

                var nextLine = reader.ReadLine();

                if (nextLine != null)
                {
                    queue.Enqueue(reader, nextLine);
                }
            }
        }
        finally
        {
            foreach (var (reader, _) in queue.UnorderedItems)
            {
                reader.Dispose();
            }
            
            var spent = (DateTime.Now - start).TotalSeconds;
            Console.WriteLine($"Finished merging in {spent:N0}s");
        }
    }
}