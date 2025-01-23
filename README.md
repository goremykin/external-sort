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
