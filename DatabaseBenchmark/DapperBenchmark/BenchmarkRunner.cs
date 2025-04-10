using Benchmarking.Models;
using System.Text;

public class BenchmarkRunner {
    private readonly DapperDatabaseBenchmark _repository;

    public BenchmarkRunner(DapperDatabaseBenchmark repository) {
        this._repository = repository;
    }

    public void RunBenchmarks() {
        var csvFilePath = Path.Combine("/app/output", "DapperBenchmarkResults.csv");
        var csvBuilder = new StringBuilder();
        csvBuilder.AppendLine("Run,MethodName,Duration(ms),MemoryUsed(bytes)");

        for (int i = 0; i < 1000; i++) {
            Console.WriteLine($"Run {i + 1}:");

            var benchmarks = new List<(string MethodName, TimeSpan Duration, long MemoryUsed)>();


            // Insert Parent with Children
            var parent = new Parent {
                Name = $"Parent_{i}",
                Children = new List<Child> {
                    new () { Name = $"Child_{i}_1" },
                    new () { Name = $"Child_{i}_2" }
                }
            };
            benchmarks.Add(BenchmarkMethod(() => _repository.InsertParentWithChildren(parent), "InsertParentWithChildren"));

            // Get Parents with Children
            benchmarks.Add(BenchmarkMethod(() => _repository.GetParentsWithChildren(), "GetParentsWithChildren"));

            // Update Parent with Children
            var latestParent = _repository.GetLatestParent();
            if (latestParent != null) {
                latestParent.Name = $"Updated_Parent_{i}";
                var childrenList = latestParent.Children.ToList();
                childrenList[0].Name = $"Updated_Child_{i}_1";
                latestParent.Children = childrenList;
                benchmarks.Add(BenchmarkMethod(() => _repository.UpdateParentWithChildren(latestParent), "UpdateParentWithChildren"));
            } else {
                Console.WriteLine("No parent found to update.");
            }

            // Delete Parent with Children
            benchmarks.Add(BenchmarkMethod(() => _repository.DeleteParentWithChildren(parent.Id), "DeleteParentWithChildren"));

            foreach (var benchmark in benchmarks) {
                csvBuilder.AppendLine($"{i + 1},{benchmark.MethodName},{benchmark.Duration.TotalMilliseconds:F4},{benchmark.MemoryUsed}");
            }


            Console.WriteLine();
        }

        // Write CSV data to file with FileShare.ReadWrite
        using (var fileStream = new FileStream(csvFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
        using (var streamWriter = new StreamWriter(fileStream, Encoding.UTF8)) {
            streamWriter.Write(csvBuilder.ToString());
        }

        Console.WriteLine($"Benchmark results saved to: {csvFilePath}");
    }

    private static (string MethodName, TimeSpan Duration, long MemoryUsed) BenchmarkMethod(Action method, string methodName) {
        var startMemory = GC.GetTotalMemory(true); // Get memory before execution
        var start = DateTime.Now;

        method();

        var end = DateTime.Now;
        var endMemory = GC.GetTotalMemory(false); // Get memory after execution

        return (methodName, end - start, endMemory - startMemory);
    }

}
