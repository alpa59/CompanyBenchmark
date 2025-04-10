using DbUp;
using Microsoft.Data.SqlClient;
using System;
using System.IO;

public static class DatabaseMigrator {
    public static void MigrateDatabase(string connectionString, bool createTables = true) {
        // Extract server connection string (without database)
        var builder = new SqlConnectionStringBuilder(connectionString) {
            InitialCatalog = string.Empty // Remove the database name
        };

        // Ensure the database exists
        EnsureDatabaseExists(builder.ConnectionString, connectionString);

        // Conditionally create tables
        if (createTables) {
            ExecuteCreateTablesScript(connectionString);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Database and tables created successfully.");
        } else {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Database created without tables.");
        }

        Console.ResetColor();
    }

    private static void EnsureDatabaseExists(string serverConnectionString, string databaseConnectionString) {
        var builder = new SqlConnectionStringBuilder(databaseConnectionString);
        var databaseName = builder.InitialCatalog;

        using var connection = new SqlConnection(serverConnectionString);
        connection.Open();

        // Check if the database exists
        var checkDatabaseExistsQuery = $"SELECT database_id FROM sys.databases WHERE Name = @DatabaseName";
        using var checkCommand = new SqlCommand(checkDatabaseExistsQuery, connection);
        checkCommand.Parameters.AddWithValue("@DatabaseName", databaseName);

        var databaseId = checkCommand.ExecuteScalar();
        if (databaseId == null) {
            Console.WriteLine($"Database '{databaseName}' does not exist. Creating it...");

            // Create the database
            var createDatabaseQuery = $"CREATE DATABASE [{databaseName}]";
            using var createCommand = new SqlCommand(createDatabaseQuery, connection);
            createCommand.ExecuteNonQuery();

            Console.WriteLine($"Database '{databaseName}' created successfully.");

            // Wait for the database to be fully initialized
            WaitForDatabaseInitialization(connection, databaseName);
        } else {
            Console.WriteLine($"Database '{databaseName}' already exists.");
        }
    }

    private static void WaitForDatabaseInitialization(SqlConnection connection, string databaseName) {
        Console.WriteLine($"Waiting for database '{databaseName}' to be fully initialized...");

        var checkDatabaseOnlineQuery = @"
        SELECT state_desc 
        FROM sys.databases 
        WHERE Name = @DatabaseName";

        while (true) {
            using var checkCommand = new SqlCommand(checkDatabaseOnlineQuery, connection);
            checkCommand.Parameters.AddWithValue("@DatabaseName", databaseName);

            var state = checkCommand.ExecuteScalar()?.ToString();
            if (state == "ONLINE") {
                Console.WriteLine($"Database '{databaseName}' is now ONLINE.");
                break;
            }

            Console.WriteLine($"Database '{databaseName}' is not yet ONLINE. Retrying in 1 second...");
            System.Threading.Thread.Sleep(1000);
        }
    }


    private static void ExecuteCreateTablesScript(string connectionString) {
        var migrationsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Migrations");
        var createTablesScriptPath = Path.Combine(migrationsFolder, "CreateTables.sql");

        if (!File.Exists(createTablesScriptPath)) {
            Console.WriteLine($"The script 'CreateTables.sql' was not found at {createTablesScriptPath}");
            throw new FileNotFoundException($"The script 'CreateTables.sql' was not found at {createTablesScriptPath}");
        }

        var scriptContent = File.ReadAllText(createTablesScriptPath);

        using var connection = new SqlConnection(connectionString);
        connection.Open();

        // Execute the script
        using var command = new SqlCommand(scriptContent, connection);
        try {
            command.ExecuteNonQuery();
            Console.WriteLine("Executed script: CreateTables.sql");
        } catch (SqlException ex) {
            Console.WriteLine($"Error executing 'CreateTables.sql': {ex.Message}");
            throw;
        }
    }
}
