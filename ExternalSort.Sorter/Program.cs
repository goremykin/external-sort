using ExternalSort.Sorter.Sorters;

if (args.Length < 2)
{
    Console.WriteLine("Please provide input and output paths");
    return;
}

var inputPath = args[0];
var outputPath = args[1];

if (!File.Exists(inputPath))
{
    Console.WriteLine("Invalid input path");
    return;
}

var startTimestamp = DateTime.Now;
var sorter = new LinuxSorter();

await sorter.SortAsync(inputPath, outputPath);

var spent = (DateTime.Now - startTimestamp).TotalMilliseconds;

Console.WriteLine($"Done in {spent} ms");