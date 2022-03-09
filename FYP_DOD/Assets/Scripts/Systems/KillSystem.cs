using Components;
using Unity.Entities;

namespace Systems
{
    public class KillSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var ecb = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer();
            
            Entities
                .WithAll<Kill>()
                .ForEach((Entity e) => 
                {
                    ecb.DestroyEntity(e); 
                }).WithoutBurst().Run();
        }
    }
}
