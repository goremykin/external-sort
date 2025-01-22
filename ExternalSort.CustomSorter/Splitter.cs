namespace ExternalSort.CustomSorter;

public static class Splitter
{
    public static IReadOnlyCollection<string> Split(string inputPath, string destDirPath, long linesPerChunk)
    {
        var start = DateTime.Now;
        var resultPaths = new List<string>();
        using var inputStream = File.OpenRead(inputPath);
        using var reader = new StreamReader(inputStream);

        while (!reader.EndOfStream)
        {
            var read = 0L;
            var outputPath = Path.Combine(destDirPath, Guid.NewGuid().ToString());
            using var outputStream = File.OpenWrite(outputPath);
            using var writer = new StreamWriter(outputStream);

            while (read < linesPerChunk && !reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }
                
                writer.WriteLine(line);
                ++read;
            }
            
            resultPaths.Add(outputPath);
        }
        
        Console.WriteLine($"Finished splitting in {(DateTime.Now - start).TotalMilliseconds}ms");
        
        return resultPaths;
    }
}