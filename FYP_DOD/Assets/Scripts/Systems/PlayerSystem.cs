using Components;
using Components.Pickups;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    public class PlayerSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var ecb = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer();
            var dt = Time.DeltaTime;
            
            var x = Input.GetAxis("Horizontal");
            var z = Input.GetAxis("Vertical");

            // Set direction based on user input
            Entities
                .WithAll<Player>()
                .ForEach((ref Movable move, ref Translation trans, in Rotation rot) =>
            {
                move.direction = new float3(x, 0, z);
            }).Schedule();

            var fire = Input.GetMouseButtonDown(0);

            // Fire bullet when user pressed left mouse button
            Entities.ForEach((ref Player player, in Translation trans, in Rotation rot) =>
            {
                if (fire && player.fireTimer >= player.fireRate)
                {
                    Entity newBullet = EntityManager.Instantiate(player.bullet);
                    EntityManager.SetComponentData(newBullet, trans);
                    EntityManager.SetComponentData(newBullet, rot);
                    player.fireTimer = 0;
                }
                else player.fireTimer += dt;
            }).WithStructuralChanges().Run();


            // If player has pick up speed boost, tick down timer for boost
            Entities
                .WithAll<Player>()
                .ForEach((Entity player, ref SpeedPickup speedPickup, ref Movable mov) =>
                {
                    speedPickup.duration -= dt;

                    if (speedPickup.duration <= 0)
                    {
                        mov.speed /= speedPickup.speedMultiplier;
                        ecb.RemoveComponent<SpeedPickup>(player);
                    }
                }).WithoutBurst().Run();

            
            // If player has pick up immunity, tick down timer for immunity
            Entities
                .WithAll<Player>()
                .ForEach((Entity player, ref ImmunityPickup immunityPickup, ref Health health) =>
                {
                    immunityPickup.duration -= dt;

                    if (immunityPickup.duration <= 0)
                    {
                        health.isImmune = false;
                        ecb.RemoveComponent<ImmunityPickup>(player);
                    }
                }).WithoutBurst().Run();
        }
    }
}
