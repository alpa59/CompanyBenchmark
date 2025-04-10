using Benchmarking;
using Benchmarking.Models;

var connectionString = "Server=mssql;Database=DapperDatabase;User Id=sa;Password=Rootr00t;TrustServerCertificate=True;";

// Run database migrations (including database creation)
DatabaseMigrator.MigrateDatabase(connectionString);

var repository = new DapperDatabaseBenchmark(connectionString);
var runner = new BenchmarkRunner(repository);

runner.RunBenchmarks();