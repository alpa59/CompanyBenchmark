using Benchmarking.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

public class DapperDatabaseBenchmark : IDatabaseBenchmark {
    private readonly string _connectionString;

    public DapperDatabaseBenchmark(string connectionString) {
        _connectionString = connectionString;
    }

    public void InsertParentWithChildren(Parent parent) {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var transaction = connection.BeginTransaction();
        try {
            // Insert Parent and get the new Id
            var parentId = connection.QuerySingle<int>(
                "INSERT INTO Parent (Name) OUTPUT INSERTED.Id VALUES (@Name);",
                new { parent.Name },
                transaction
            );

            // Insert Children
            foreach (var child in parent.Children) {
                connection.Execute(
                    "INSERT INTO Child (Name, ParentId) VALUES (@Name, @ParentId);",
                    new { child.Name, ParentId = parentId },
                    transaction
                );
            }

            transaction.Commit();
        } catch (Exception ex) {
            transaction.Rollback();
            Console.WriteLine($"Error inserting Parent with Children: {ex.Message}");
            throw;
        }
    }

    public Parent GetLatestParent() {
        using var connection = new SqlConnection(_connectionString);
        var parent = connection.Query<Parent>(
            @"SELECT TOP 1 * FROM Parent ORDER BY Id DESC;"
        ).FirstOrDefault();

        if (parent != null) {
            parent.Children = connection.Query<Child>(
                @"SELECT * FROM Child WHERE ParentId = @ParentId;",
                new { ParentId = parent.Id }
            ).ToList();
        }

        return parent;
    }


    public List<Parent> GetParentsWithChildren() {
        using var connection = new SqlConnection(_connectionString);
        var parentDictionary = new Dictionary<int, Parent>();

        var parents = connection.Query<Parent, Child, Parent>(
            @"SELECT p.Id, p.Name, c.Id, c.Name, c.ParentId
              FROM Parent p
              LEFT JOIN Child c ON p.Id = c.ParentId;",
            (parent, child) => {
                if (!parentDictionary.TryGetValue(parent.Id, out var parentEntry)) {
                    parentEntry = parent;
                    parentEntry.Children = new List<Child>();
                    parentDictionary.Add(parent.Id, parentEntry);
                }

                if (child != null && child.Id != 0) // guard against default value of int
                {
                    parentEntry.Children.Add(child);
                }

                return parentEntry;
            },
            splitOn: "Id"
        );

        return parentDictionary.Values.ToList();
    }

    public void UpdateParentWithChildren(Parent parent) {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var transaction = connection.BeginTransaction();
        try {
            // Check if Parent exists
            var parentExists = connection.ExecuteScalar<int>(
                "SELECT COUNT(1) FROM Parent WHERE Id = @Id;",
                new { parent.Id },
                transaction
            ) > 0;

            if (!parentExists) {
                throw new InvalidOperationException($"Parent with Id {parent.Id} does not exist.");
            }

            // Update Parent
            connection.Execute(
                "UPDATE Parent SET Name = @Name WHERE Id = @Id;",
                new { parent.Name, parent.Id },
                transaction
            );

            // Get existing children
            var existingChildren = connection.Query<Child>(
                "SELECT Id FROM Child WHERE ParentId = @ParentId;",
                new { ParentId = parent.Id },
                transaction
            ).ToList();

            var existingChildIds = existingChildren.Select(c => c.Id).ToHashSet();

            // Update or insert children
            foreach (var child in parent.Children) {
                if (existingChildIds.Contains(child.Id)) {
                    connection.Execute(
                        "UPDATE Child SET Name = @Name WHERE Id = @Id;",
                        new { child.Name, child.Id },
                        transaction
                    );
                } else {
                    connection.Execute(
                        "INSERT INTO Child (Name, ParentId) VALUES (@Name, @ParentId);",
                        new { child.Name, ParentId = parent.Id },
                        transaction
                    );
                }
            }

            transaction.Commit();
        } catch {
            transaction.Rollback();
            throw;
        }
    }


    public void DeleteParentWithChildren(int parentId) {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var transaction = connection.BeginTransaction();
        try {
            connection.Execute(
                "DELETE FROM Child WHERE ParentId = @ParentId;",
                new { ParentId = parentId },
                transaction
            );

            connection.Execute(
                "DELETE FROM Parent WHERE Id = @Id;",
                new { Id = parentId },
                transaction
            );

            transaction.Commit();
        } catch {
            transaction.Rollback();
            throw;
        }
    }
}
