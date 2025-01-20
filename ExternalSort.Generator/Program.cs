using ExternalSort.Generator;

var fileSizeBytes = args.Length > 0 && long.TryParse(args[0], out var num)
    ? num
    : 3_221_225_472; // 3 GiB

Console.WriteLine($"Generating a file of size {fileSizeBytes:N0} bytes");

var startTimestamp = DateTime.Now;
var wordRandomizer = new WordRandomizer();
var writtenBytes = 0L;
using var stream = new FileStream("input.txt", FileMode.Create, FileAccess.Write, FileShare.None);
using var writer = new StreamWriter(stream);

while (writtenBytes < fileSizeBytes)
{
    var number = Random.Shared.Next(1, 1000000);
    var adjective = wordRandomizer.NextAdjective();
    var noun = wordRandomizer.NextNoun();
    var line = $"{number}. {adjective} {noun}";
    
    writer.WriteLine(line);
    
    writtenBytes += line.Length;
}

var spentMs = (DateTime.Now - startTimestamp).TotalMilliseconds;

Console.WriteLine($"Done in {spentMs:N} ms");