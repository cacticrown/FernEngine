namespace FernEngine.ECS;

public interface ISparseSetBase
{
    void Remove(int id);
    bool Has(int id);
}

public interface ISparseSet<T> : ISparseSetBase
{
    void Add(int id, T component);
    ref T Get(int id);
}