using Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

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
                
                if (!move.direction.Equals(float3.zero)) 
                    rot.Value = Quaternion.Slerp(rot.Value, Quaternion.LookRotation(move.direction), 0.15f);
            }).Schedule();
        }
    }
}
