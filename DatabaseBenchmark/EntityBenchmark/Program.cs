using EntityBenchmark;

var context = new CompanyBenchmarkContext();
context.EnsureDatabaseCreated();

var benchmarkRunner = new BenchmarkRunner();
benchmarkRunner.RunBenchmarks();