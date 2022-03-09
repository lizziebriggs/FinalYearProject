using Components;
using Unity.Entities;

namespace Systems
{
    public class HealthSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var dt = Time.DeltaTime;
            
            var ecb = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer();
            
            Entities
                .WithAll<Health>()
                .ForEach((Entity e, ref Health health) =>
                {
                    if (health.damageTimer > 0)
                        health.damageTimer -= dt;
                    
                    if (health.health <= 0)
                        ecb.AddComponent<Kill>(e);
                }).WithoutBurst().Run();
        }
    }
}
