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
            sparseSets.Add(type, new SparseSet<T>());
        }
        GetSparseSet<T>().Add(id, component);
    }

    public void Remove<T>(int id) => GetSparseSet<T>().Remove(id);
    public T Get<T>(int id) => GetSparseSet<T>().Get(id);
    public bool Has<T>(int id) => GetSparseSet<T>().Has(id);
    public bool HasSparseSet<T>() => sparseSets .ContainsKey(typeof(T));
    private SparseSet<T> GetSparseSet<T>() => (SparseSet<T>)sparseSets[typeof(T)];
}