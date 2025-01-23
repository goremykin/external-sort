using System.CommandLine;
using ExternalSort.Sorter;

var inputOption = new Option<string>("--input")
{
    Description = "Path to an input file",
    IsRequired = true
};
inputOption.AddAlias("-i");
inputOption.AddValidator(result  =>
{
    var inputPath = result.GetValueOrDefault<string>();

    if (!File.Exists(inputPath))
    {
        result.ErrorMessage = "Invalid input path";
    }
});

var outputOption = new Option<string>("--output")
{
    Description = "Path to an output file",
};
outputOption.AddAlias("-o");
outputOption.SetDefaultValue("output.txt");

var sorterOption = new Option<string>("--sorter")
{
    Description = "Sorter implementation. Possible values: custom, linux. Custom is manually implemented, Linux uses built-in linux sort utility",
};
sorterOption.AddAlias("-s");
sorterOption.SetDefaultValue("custom");

var memoryOption = new Option<int?>("--memory")
{
    Description = "The amount of RAM that can be used for sorting. Will be determined automatically if not specified"
};
memoryOption.AddAlias("-m");

var parallelismOption = new Option<int?>("--parallelism")
{
    Description = "The number of cores that can be used for sorting. Will be determined automatically if not specified"
};
parallelismOption.AddAlias("-p");

var rootCommand = new RootCommand("Performs sorting of a large file")
{
    inputOption,
    outputOption,
    sorterOption,
    memoryOption,
    parallelismOption,
};

rootCommand.SetHandler(RootCommandHandler.Handle, inputOption, outputOption, sorterOption, memoryOption, parallelismOption);

rootCommand.Invoke(args);