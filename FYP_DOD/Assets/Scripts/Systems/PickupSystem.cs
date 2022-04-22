using Components;
using Components.Pickups;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    public class PickupSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var dt = Time.DeltaTime;
            var ecb = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer();
            
            Entities
                .WithAll<Player>()
                .ForEach((Entity player, DynamicBuffer<TriggerBuffer> trigger, ref Movable move, ref Health health) =>
            {
                for (int i = 0; i < trigger.Length; i++)
                {
                    // Apply speed boost effect
                    if (HasComponent<SpeedPickup>(trigger[i].entity))
                    {
                        move.speed *= GetComponent<SpeedPickup>(trigger[i].entity).speedMultiplier;
                        ecb.AddComponent(player, GetComponent<SpeedPickup>(trigger[i].entity));
                        ecb.DestroyEntity(trigger[i].entity);
                    }
                    
                    // Apply immunity effect
                    if (HasComponent<ImmunityPickup>(trigger[i].entity))
                    {
                        health.isImmune = true;
                        ecb.AddComponent(player, GetComponent<ImmunityPickup>(trigger[i].entity));
                        ecb.DestroyEntity(trigger[i].entity);
                    }
                }
            }).WithoutBurst().Run();

            Entities
                .WithAll<IdleRotate>()
                .ForEach((ref Translation trans, ref Rotation rot) =>
                {
                    var rotMove = quaternion.AxisAngle(new float3(1f, 0f, 0f), 3f * dt);
                    rot.Value = math.mul(rotMove, rot.Value);
                }).Schedule();
        }
    }
}
