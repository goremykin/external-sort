using System.CommandLine;
using ExternalSort.Generator;

var sizeOption = new Option<int>("--size")
{
    Description = "File size in gigabytes",
    
};
sizeOption.SetDefaultValue(10);
sizeOption.AddAlias("-s");

var outputOption = new Option<string>("--output")
{
    Description = "Output file path"
};
outputOption.SetDefaultValue("input.txt");
outputOption.AddAlias("-o");

var rootCommand = new RootCommand("Generates a file for external sort")
{
    sizeOption,
    outputOption
};

rootCommand.SetHandler(RootCommandHandler.Handle, sizeOption, outputOption);
await rootCommand.InvokeAsync(args);