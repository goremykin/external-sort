using ExternalSort.Contracts;

namespace ExternalSort.Sorter;

public static class RootCommandHandler
{
    public static async Task Handle(string inputPath, string outputPath, string sorterKey, int? memoryMb, int? parallelism)
    {
        memoryMb ??= ResourceCalculator.GetUsableMemoryMb();
        parallelism ??= ResourceCalculator.GetUsableCores();
        
        ISorter sorter = sorterKey switch
        {
            Sorters.Linux => new LinuxSorter.LinuxSorter(memoryMb.Value, parallelism.Value),
            _ => new CustomSorter.CustomSorter(memoryMb.Value, parallelism.Value)
        };
        
        Console.WriteLine($"Starting sorting with {sorter.GetType().Name} (Memory MB: {memoryMb}, Parallelism: {parallelism})");
        
        var startTimestamp = DateTime.Now;
        
        await sorter.SortAsync(inputPath, outputPath);
        
        var spent = (DateTime.Now - startTimestamp).TotalSeconds;
        
        Console.WriteLine($"Done in {spent:N0}s");
    }
}