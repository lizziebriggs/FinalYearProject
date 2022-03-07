using Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    public class BulletSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var dt = Time.DeltaTime;
            
            var ecb = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer();
            
            Entities
                .WithAll<Bullet>()
                .ForEach((ref Movable move, ref Translation trans, in Rotation rot) =>
                {
                    move.direction = math.forward(rot.Value);
                }).Schedule();
            
            Entities
                .WithAll<Bullet>()
                .ForEach((Entity e, ref Bullet bullet) =>
            {
                bullet.timer += dt;
                if (bullet.timer >= bullet.lifeSpan)
                    ecb.DestroyEntity(e);
            }).Schedule();
        }
    }
}
