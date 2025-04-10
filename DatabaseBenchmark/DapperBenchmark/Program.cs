using Microsoft.Data.SqlClient;
using System;
using System.Threading;

var connectionString = "Server=mssql;Database=DapperDatabase;User Id=sa;Password=Rootr00t;TrustServerCertificate=True;";

// Ensure the database exists without creating tables
DatabaseMigrator.MigrateDatabase(connectionString);

// Retry logic to ensure SQL Server is ready
var retries = 10;
while (retries > 0) {
    try {
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        Console.WriteLine("Connected to SQL Server successfully.");
        break;
    } catch (SqlException ex) {
        Console.WriteLine($"SQL Server not ready. Retrying... ({retries} attempts left)");
        retries--;
        Thread.Sleep(5000); // Wait 5 seconds before retrying
        if (retries == 0) throw new Exception("Unable to connect to SQL Server after multiple attempts.", ex);
    }
}

var repository = new DapperDatabaseBenchmark(connectionString);
var runner = new BenchmarkRunner(repository);
runner.RunBenchmarks();