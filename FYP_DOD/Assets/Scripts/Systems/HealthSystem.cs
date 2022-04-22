using Components;
using Mono;
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
                    // Count damage delay to stop damage being applied for multiple
                    // consecutive updates
                    if (health.damageTimer > 0)
                        health.damageTimer -= dt;
                    
                    if (health.health <= 0)
                        ecb.AddComponent<Kill>(e);
                }).WithoutBurst().Run();
            
            
            // Update health to display in UI
            Entities
                .WithAll<Player>()
                .ForEach((ref Health health) =>
                {
                    GameManager.instance.UpdateHealth(health.health);
                }).WithoutBurst().Run();
        }
    }
}
