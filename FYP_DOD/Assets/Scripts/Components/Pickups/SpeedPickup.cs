using Unity.Entities;

namespace Components.Pickups
{
    [GenerateAuthoringComponent]
    public struct SpeedPickup : IComponentData
    {
        public float speedMultiplier;
        public float duration;
    }
}

