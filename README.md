## The problem

```
The input is a large text file, where each line is "Number. String"
For example:
415. Apple
30432. Something something something
1. Apple
32. Cherry is the best
2. Banana is yellow

Both parts can be repeated within the file. You need to get another file as output, where all
the lines are sorted. Sorting criteria: String part is compared first, if it matches then
Number.

Those in the example above, it should be:
1. Apple
415. Apple
2. Banana is yellow
32. Cherry is the best
30432. Something something something

You need to write two programs:
1. A utility for creating a test file of a given size. The result of the work should be a text file
of the type described above. There must be some number of lines with the same String
part.
2. The actual sorter. An important point, the file can be very large. The size of ~100Gb will
be used for testing.
```

## How build

```shell
cd external-sort
dotnet publish ./ExternalSort.Generator -c Release -o ./publish
dotnet publish ./ExternalSort.Sorter -c Release -o ./publish
cd ./publish
```

## How to use

```shell
# Use --help for more details
./ExternalSort.Generator --size 10 --output ./10GB.txt

# Use --help for more details. You can specify memory and number of cores if you need
./ExternalSort.Sorter --input ./10GB.txt --output ./output.txt

# If you use linux you can try to use built-in sort utility for comparison
./ExternalSort.Sorter --sorter linux --input ./10GB.txt --output ./output.txt
```

## Benchmarks
| Sorter                | Input size  | Time          |
|-----------------------|-------------|---------------|
| Custom implementation | 10GB        | 724 seconds   |
| Linux sort            | 10GB        | 584 seconds   |
