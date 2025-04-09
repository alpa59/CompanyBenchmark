using EntityBenchmark;
using EntityBenchmark.Entity;

var context = new EntityContext();
context.EnsureDatabaseCreated(); // Ensure the database is created
var repository = new EntityDatabaseBenchmark(context);
var runner = new BenchmarkRunner(repository);
runner.RunBenchmarks();

