namespace FernEngine.ECS;

public class EntityStore
{
    public Entity[] Entities;
    public int nextId = 0;
    private Stack<int> FreeIds;

    private ComponentStore componentStore;

    public EntityStore(int size, ComponentStore ComponentStore)
    {
        Entities = new Entity[size];
        FreeIds = new Stack<int>();
        componentStore = ComponentStore;
    }

    public Entity Create(params IComponent[] components)
    {
        int id;

        if (FreeIds.Count > 0)
            id = FreeIds.Pop();
        else
            id = nextId++;

        if (id >= Entities.Length)
            Resize(Entities.Length * 2);

        Entity entity = new Entity(id, this, componentStore);
        Entities[id] = entity;
        foreach (var component in components)
        {
            componentStore.Add(id, component);
        }
        return entity;
    }

    public void Resize(int newSize) => Array.Resize(ref Entities, newSize);

    public void Remove(int id)
    {
        if (!Has(id))
            throw new Exception($"Tried to remove non-existing Entity {id}");

        FreeIds.Push(id);

        Entities[id] = default;
        componentStore.RemoveAll(id);
    }

    public ref Entity Get(int id)
    {
        if (!Has(id))
            throw new Exception($"Tried to get non-existing Entity {id}");
        return ref Entities[id];
    }

    public bool Has(int id) => id < Entities.Length && !Entities[id].Equals(default(Entity));
}
