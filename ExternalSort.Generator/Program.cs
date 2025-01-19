using ExternalSort.Generator;

var numberOfLines = args.Length > 1 && int.TryParse(args[1], out var num)
    ? num
    : 100_000_000;

Console.WriteLine($"Generating {numberOfLines:N0} lines");

var startTimestamp = DateTime.Now;
var wordRandomizer = new WordRandomizer();
using var stream = new FileStream("input.txt", FileMode.Create, FileAccess.Write, FileShare.None);
using var writer = new StreamWriter(stream);

for (var i = 0; i < numberOfLines; ++i)
{
    var number = Random.Shared.Next(1, 1000000);
    var adjective = wordRandomizer.NextAdjective();
    var noun = wordRandomizer.NextNoun();
    
    writer.WriteLine($"{number}. {adjective} {noun}");
}

var spentMs = (DateTime.Now - startTimestamp).TotalMilliseconds;

Console.WriteLine($"Done in {spentMs:N} ms");