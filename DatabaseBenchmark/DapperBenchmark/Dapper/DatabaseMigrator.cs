using DbUp;
using Microsoft.Data.SqlClient;
using System;
using System.Data.SqlClient;
using System.IO;

public static class DatabaseMigrator {
    public static void MigrateDatabase(string connectionString) {
        // Extract server connection string (without database)
        var builder = new SqlConnectionStringBuilder(connectionString) {
            InitialCatalog = string.Empty // Remove the database name
        };

        // Execute the init.sql script to create the database if it doesn't exist
        ExecuteInitScript(builder.ConnectionString);

        // Run migrations
        var upgrader = DeployChanges.To
            .SqlDatabase(connectionString)
            .WithScriptsFromFileSystem("Migrations")
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(result.Error);
            Console.ResetColor();
            throw new Exception("Database migration failed.");
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Database migration successful.");
        Console.ResetColor();
    }

    private static void ExecuteInitScript(string serverConnectionString) {
        var migrationsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Migrations");

        // Define the scripts to execute in order
        var scripts = new[] { "CreateDatabase.sql", "CreateTables.sql" };

        using var connection = new SqlConnection(serverConnectionString);
        connection.Open();

        foreach (var script in scripts) {
            var scriptPath = Path.Combine(migrationsFolder, script);
            if (!File.Exists(scriptPath)) {
                Console.WriteLine($"The script {script} was not found at {scriptPath}");
                throw new FileNotFoundException($"The script {script} was not found at {scriptPath}");
            }

            var scriptContent = File.ReadAllText(scriptPath);

            // Execute the script
            using var command = new SqlCommand(scriptContent, connection);
            try {
                command.ExecuteNonQuery();
                Console.WriteLine($"Executed script: {script}");
            } catch (SqlException ex) when (ex.Number == 1801) // Database already exists
            {
                Console.WriteLine($"Skipping script {script}: {ex.Message}");
            }
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Database initialization scripts executed successfully.");
        Console.ResetColor();
    }



}
