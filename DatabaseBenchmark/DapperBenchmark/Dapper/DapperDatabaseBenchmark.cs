using Benchmarking.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlClient;

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
            // Insert Parent
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
        } catch {
            transaction.Rollback();
            throw;
        }
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
                if (child != null) {
                    parentEntry.Children.Add(child);
                }
                return parentEntry;
            },
            splitOn: "Id"
        );

        return parents.Distinct().ToList();
    }


    public void UpdateParentWithChildren(Parent parent) {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var transaction = connection.BeginTransaction();
        try {
            // Update Parent
            connection.Execute(
                "UPDATE Parent SET Name = @Name WHERE Id = @Id;",
                new { parent.Name, parent.Id },
                transaction
            );

            // Update Children
            foreach (var child in parent.Children) {
                connection.Execute(
                    "UPDATE Child SET Name = @Name WHERE Id = @Id;",
                    new { child.Name, child.Id },
                    transaction
                );
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
            // Delete Children
            connection.Execute(
                "DELETE FROM Child WHERE ParentId = @ParentId;",
                new { ParentId = parentId },
                transaction
            );

            // Delete Parent
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
