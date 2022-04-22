using Unity.Entities;

namespace Components.Pickups
{
    [GenerateAuthoringComponent]
    public struct ImmunityPickup : IComponentData
    {
        public float duration;
    }
}
