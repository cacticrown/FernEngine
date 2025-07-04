namespace FernEngine.ECS;

internal interface ISparseSetBase
{
    void Remove(int id);
    bool Has(int id);
}

internal interface ISparseSet<T> : ISparseSetBase
{
    void Add(int id, T component);
    ref T Get(int id);
}