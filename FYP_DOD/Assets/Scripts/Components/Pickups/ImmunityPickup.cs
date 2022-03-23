using Unity.Entities;
using Unity.Mathematics;

namespace Components.Pickups
{
    [GenerateAuthoringComponent]
    public struct ImmunityPickup : IComponentData
    {
        public float duration;
    }
}
