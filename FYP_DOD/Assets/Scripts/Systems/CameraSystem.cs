using Components;
using Unity.Entities;
using Unity.Transforms;

namespace Systems
{
    public class CameraSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref Translation translation, ref Rotation rotation, in Camera camera) => {
                
            }).Schedule();
        }
    }
}
