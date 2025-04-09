using EntityBenchmark.Models;
using System.Text;

public class BenchmarkRunner {
    private readonly IDatabaseBenchmark _repository;

    public BenchmarkRunner(IDatabaseBenchmark repository) {
        this._repository = repository;
    }

    public void RunBenchmarks(string csvFilePath) {
        var csvBuilder = new StringBuilder();
        csvBuilder.AppendLine("Run;MethodName;Duration(ms)");

        for (int i = 0; i < 1000; i++) {
            Console.WriteLine($"Run {i + 1}:");

            var benchmarks = new List<(string MethodName, TimeSpan Duration)>();

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
            parent.Name = $"Updated_Parent_{i}";
            var childrenList = parent.Children.ToList();
            childrenList[0].Name = $"Updated_Child_{i}_1";
            parent.Children = childrenList;
            benchmarks.Add(BenchmarkMethod(() => _repository.UpdateParentWithChildren(parent), "UpdateParentWithChildren"));

            // Delete Parent with Children
            benchmarks.Add(BenchmarkMethod(() => _repository.DeleteParentWithChildren(parent.Id), "DeleteParentWithChildren"));

            foreach (var benchmark in benchmarks) {
                csvBuilder.AppendLine($"{i + 1};{benchmark.MethodName};{benchmark.Duration.TotalMilliseconds:F4}");
            }

            Console.WriteLine();
        }

        // Write CSV data to file
        File.WriteAllText(csvFilePath, csvBuilder.ToString());
        Console.WriteLine($"Benchmark results saved to: {csvFilePath}");
    }

    private static (string MethodName, TimeSpan Duration) BenchmarkMethod(Action method, string methodName) {
        var start = DateTime.Now;
        method();
        var end = DateTime.Now;
        return (methodName, end - start);
    }
}
