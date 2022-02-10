using Components;
using Unity.Entities;
using Unity.Transforms;

namespace Systems
{
    public class MovableSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float dt = Time.DeltaTime;
            
            Entities.ForEach((ref Movable move, ref Translation trans, ref Rotation rot) =>
            {
                trans.Value += move.speed * move.direction * dt;
            }).Schedule();
        }
    }
}
