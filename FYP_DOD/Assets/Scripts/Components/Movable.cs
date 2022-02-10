using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct Movable : IComponentData
    {
        public float speed;
        public float3 direction;
    }
}
