using Unity.Entities;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct Player : IComponentData
    {
        public Entity bullet;
        public float fireRate;
        public float fireTimer;
    }
}
