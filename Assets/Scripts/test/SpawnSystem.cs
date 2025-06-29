using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct SpawnSystem : ISystem
{
    public void OnCreate(ref SystemState state)
        => state.RequireForUpdate<Config>();
    
    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<Config>();

        var instances = state.EntityManager.Instantiate
            (config.Prefab, config.SpawnCount, Allocator.Temp);


        var rand = new Random(config.RandomSeed);
        foreach (var entity in instances)
        {
            var xform = SystemAPI.GetComponentRW<LocalTransform>(entity);
            var walker = SystemAPI.GetComponentRW<Walker>(entity);

            xform.ValueRW = LocalTransform.FromPositionRotation
                (rand.NextOnDisk() * config.SpawnRadius, rand.NextYRotation());

            walker.ValueRW = Walker.Create(ref rand);;
        }

        state.Enabled = false;
    }
}