using Components;
using Unity.Entities;

namespace Systems
{
    public class DamageSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var dt = Time.DeltaTime;
            var ecb = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer();
            
            Entities.ForEach((DynamicBuffer<CollisionBuffer> collision, ref Health health) => {
                for (int i = 0; i < collision.Length; i++)
                {
                    if (health.damageTimer <= 0 && !health.isImmune && HasComponent<Damage>(collision[i].entity))
                    {
                        health.health -= GetComponent<Damage>(collision[i].entity).damage;
                        health.damageTimer = health.damageDelay;
                    }
                }
            }).Schedule();
            
            Entities.ForEach((Entity e, DynamicBuffer<TriggerBuffer> trigger, ref Health health) => {
                for (int i = 0; i < trigger.Length; i++)
                {
                    if (health.damageTimer <= 0 && !health.isImmune && HasComponent<Damage>(trigger[i].entity))
                    {
                        health.health -= GetComponent<Damage>(trigger[i].entity).damage;
                        health.damageTimer = health.damageDelay;
                    }
                }
            }).Schedule();
        }
    }
}
