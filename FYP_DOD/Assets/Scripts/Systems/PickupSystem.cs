using Components;
using Components.Pickups;
using Unity.Entities;

namespace Systems
{
    public class PickupSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var ecb = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer();
            
            Entities
                .WithAll<Player>()
                .ForEach((Entity player, DynamicBuffer<TriggerBuffer> trigger, ref Movable move, ref Health health) =>
            {
                for (int i = 0; i < trigger.Length; i++)
                {
                    if (HasComponent<SpeedPickup>(trigger[i].entity))
                    {
                        move.speed *= GetComponent<SpeedPickup>(trigger[i].entity).speedMultiplier;
                        ecb.AddComponent(player, GetComponent<SpeedPickup>(trigger[i].entity));
                        ecb.DestroyEntity(trigger[i].entity);
                    }
                    
                    if (HasComponent<ImmunityPickup>(trigger[i].entity))
                    {
                        health.isImmune = true;
                        ecb.AddComponent(player, GetComponent<ImmunityPickup>(trigger[i].entity));
                        ecb.DestroyEntity(trigger[i].entity);
                    }
                }
            }).WithoutBurst().Run();
        }
    }
}
