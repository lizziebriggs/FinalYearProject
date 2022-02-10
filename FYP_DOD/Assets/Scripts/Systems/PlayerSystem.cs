using Components;
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
            var x = Input.GetAxis("Horizontal");
            var z = Input.GetAxis("Vertical");

            Entities.ForEach((ref Movable move, ref Translation trans, in Rotation rot, in Player player) =>
            {
                move.direction = new float3(x, 0, z);
            }).Schedule();
        }
    }
}
