namespace FernEngine.ECS;

public struct Entity
{
    public int Id;
    private EntityStore entityStore;
    private ComponentStore componentStore;

    public Entity(int id, EntityStore EntityStore, ComponentStore ComponentStore)
    {
        Id = id;
        entityStore = EntityStore;
        componentStore = ComponentStore;
    }

    public void Add<T>(T component)
    {
        componentStore.Add(Id, component);
    }

    public void Remove<T>()
    {
        componentStore.Remove<T>(Id);
    }

    public void Destroy()
    {
        entityStore.Remove(Id);
    }
}
