namespace FernEngine.ECS;

public struct SparseSet<T> : ISparseSet<T>
{
    internal T[] dense;          // stores components
    private int[] sparse;       // maps entity ID → dense index
    private int[] entityIds;   // reverse map, index: dense index, value: entity ID

    internal int nextId;

    public SparseSet(int size)
    {
        dense = new T[size];
        sparse = new int[size];
        entityIds = new int[size];
        nextId = 0;

        for (int i = 0; i < sparse.Length; i++)
            sparse[i] = -1;
    }

    public ref T this[int id]
    {
        get
        {
            return ref dense[sparse[id]];
        }
    }

    public void Add(int id, T component)
    {
        if (Has(id))
            throw new Exception($"Entity {id} already has a component of type {typeof(T)}");

        if (nextId >= dense.Length)
            ResizeDense(dense.Length * 2);

        if (id >= sparse.Length)
            ResizeSparse(Math.Max(sparse.Length * 2, id * 2));

        sparse[id] = nextId;
        entityIds[nextId] = id;
        dense[nextId] = component;

        nextId++;
    }

    private void ResizeDense(int newSize)
    {
        Array.Resize(ref dense, newSize);
        Array.Resize(ref entityIds, newSize);
    }   
    
    private void ResizeSparse(int newSize)
    {
        int oldSize = sparse.Length;
        Array.Resize(ref sparse, newSize);
        for (int i = oldSize; i < newSize; i++)
            sparse[i] = -1;
    }


    public void Remove(int id)
    {
        if (!Has(id))
            return;

        // move the last component into the removed slot
        dense[sparse[id]] = dense[nextId - 1];
        entityIds[sparse[id]] = entityIds[nextId - 1];

        // update sparse to point to new component index
        sparse[entityIds[sparse[id]]] = sparse[id];

        // clear removed slot
        dense[nextId - 1] = default;
        entityIds[nextId - 1] = -1;
        sparse[id] = -1;

        nextId--;
    }

    public ref T Get(int id)
    {
        if (!Has(id))
            throw new Exception($"Tried to get component {typeof(T)} from entity {id}, which does not have the component.");
        return ref dense[sparse[id]];
    }

    public bool Has(int id)
    {
        return id >= 0 && id < sparse.Length && sparse[id] != -1;
    }

    public IEnumerable<T> GetAll()
    {
        for (int i = 0; i < nextId; i++)
        {
            yield return dense[i];
        }
    }
}
