namespace FernEngine.ECS;

public class ComponentStore
{
    private Dictionary<Type, ISparseSetBase> sparseSets;

    public ComponentStore()
    {
        sparseSets = new Dictionary<Type, ISparseSetBase>();
    }

    public void Add<T>(int id, T component)
    {
        var type = typeof(T);
        if (!HasSparseSet<T>())
        {
            sparseSets.Add(type, new SparseSet<T>(256));
        }
        GetSparseSet<T>().Add(id, component);
    }

    public void AddSparseSet(ISparseSetBase sparseSet)
    {
        var type = sparseSet.GetType().GetGenericArguments()[0];
        if (!sparseSets.ContainsKey(type))
        {
            sparseSets.Add(type, sparseSet);
        }
        else
        {
            throw new Exception($"Sparse set for type {type} already exists.");
        }
    }

    public void Remove<T>(int id) => GetSparseSet<T>().Remove(id);

    public void RemoveAll(int id)
    {
        foreach (var sparseSet in sparseSets.Values)
        {
            sparseSet.Remove(id);
        }
    }

    public T Get<T>(int id) => GetSparseSet<T>().Get(id);

    public bool Has<T>(int id) => GetSparseSet<T>().Has(id);

    public bool HasSparseSet<T>() => sparseSets .ContainsKey(typeof(T));

    private SparseSet<T> GetSparseSet<T>() => (SparseSet<T>)sparseSets[typeof(T)];
}