using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using FernEngine.ECS;

namespace FernEngine.Test;

internal class Program
{
    static readonly int entityAmount = 10;

    static void Main(string[] args)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        ComponentStore componentStore = new ComponentStore();
        EntityStore entityStore = new EntityStore(entityAmount, componentStore);

        stopwatch.Stop();
        Console.WriteLine($"Time taken to init World: {stopwatch.Elapsed.TotalMilliseconds} ms");

        stopwatch.Start();

        componentStore.AddSparseSet(new SparseSet<TestComponent>(entityAmount));

        stopwatch.Stop();
        Console.WriteLine($"Time taken to pre-init sparseset: {stopwatch.Elapsed.TotalMilliseconds} ms");

        stopwatch.Start();

        for(int i = 0; i < entityAmount; i++)
        {
            entityStore.Create();
        }

        stopwatch.Stop();
        Console.WriteLine($"Time taken to create {entityAmount}: {stopwatch.Elapsed.TotalMilliseconds} ms");

        stopwatch.Start();

        for (int i = 0; i < entityAmount; i++)
        {
            entityStore.Create(new TestComponent());
        }

        stopwatch.Stop();
        Console.WriteLine($"Time taken to create {entityAmount} with 1 component: {stopwatch.Elapsed.TotalMilliseconds} ms");

        Console.ReadKey();
    }
}

struct TestComponent : IComponent
{

}