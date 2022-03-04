using Unity.Entities;
using Unity.Mathematics;

namespace Components.Pickups
{
    [GenerateAuthoringComponent]
    public struct SpeedPickup : IComponentData
    {
        public float speedMultiplier;
        public float duration;
    }
}

