namespace ExternalSort.Generator;

public static class RootCommandHandler
{
    public static void Handle(int sizeGb, string outputPath)
    {
        Console.WriteLine($"Generating a file of size {sizeGb} GB");
        
        var startTimestamp = DateTime.Now;
        var wordRandomizer = new WordRandomizer();
        var fileSizeBytes = (long)sizeGb * 1024 * 1024 * 1024;
        var writtenBytes = 0L;
        using var writer = new StreamWriter("input.txt", append: false);

        while (writtenBytes < fileSizeBytes)
        {
            var number = Random.Shared.Next(1, 1000000);
            var adjective = wordRandomizer.NextAdjective();
            var noun = wordRandomizer.NextNoun();
            var line = $"{number}. {adjective} {noun}";
    
            writer.WriteLine(line);
    
            writtenBytes += line.Length;
        }

        var spentMs = (DateTime.Now - startTimestamp).TotalSeconds;

        Console.WriteLine($"Done in {spentMs:N0}s");
    }
}