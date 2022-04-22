using Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

namespace Systems
{
    public class EnemySystem : SystemBase
    {
        private Random rng = new Random(1234);
        
        protected override void OnUpdate()
        {
            var raycaster = new MovementRaycast()
            {
                pw = World.GetOrCreateSystem<BuildPhysicsWorld>().PhysicsWorld
            };

            rng.NextInt();
            var rngTemp = rng;
            
            Entities.ForEach((ref Movable mov, ref Translation trans, ref Enemy enemy) =>
            {
                bool hitWall = raycaster.CheckRay(trans.Value, mov.direction, mov.direction);

                if (hitWall)
                {
                    var validDir = new NativeList<float3>(Allocator.Temp);
                    
                    // Cast raycast in each direction from entity and add to list of valid
                    // directions if it does not hit a wall
                    if (!raycaster.CheckRay(trans.Value, new float3(0, 0, -1), mov.direction))
                        validDir.Add(new float3(0, 0, -1));
                    if (!raycaster.CheckRay(trans.Value, new float3(0, 0, 1), mov.direction))
                        validDir.Add(new float3(0, 0, 1));
                    if (!raycaster.CheckRay(trans.Value, new float3(-1, 0, 0), mov.direction))
                        validDir.Add(new float3(-1, 0, 0));
                    if (!raycaster.CheckRay(trans.Value, new float3(1, 0, 0), mov.direction))
                        validDir.Add(new float3(1, 0, 0));

                    // If there are no valid directions, reverse the enemy movement
                    if (validDir.Length == 0)
                        mov.direction = -mov.direction;
                    else
                        mov.direction = validDir[rngTemp.NextInt(validDir.Length)];
                    
                    validDir.Dispose();
                }

            }).WithBurst().ScheduleParallel();
        }

        private struct MovementRaycast
        {
            [ReadOnly] public PhysicsWorld pw;

            public bool CheckRay(float3 pos, float3 dir, float3 currentDir)
            {
                // Do not include direction the entity just came from
                if (dir.Equals(-currentDir))
                    return true;

                var ray = new RaycastInput()
                {
                    Start = pos,
                    End = pos + dir * 3f,
                    // Collision filter sets enemies to belong to enemies,
                    // and collides with walls
                    Filter = new CollisionFilter()
                    {
                        GroupIndex = 0,
                        BelongsTo = 1u << 1,
                        CollidesWith = 1u << 2
                    }
                };

                return pw.CastRay(ray);
            }
        }
    }
}
