using EntityBenchmark.Models;

public interface IDatabaseBenchmark {
    void InsertParentWithChildren(Parent parent);
    List<Parent> GetParentsWithChildren();
    void UpdateParentWithChildren(Parent parent);
    void DeleteParentWithChildren(int parentId);
}
