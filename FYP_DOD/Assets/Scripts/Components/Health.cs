using Unity.Entities;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct Health : IComponentData
    {
        public int health;
        public float damageDelay;
        public float damageTimer;
        public bool isImmune;
    }
}
