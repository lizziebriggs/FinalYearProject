using Unity.Entities;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct Bullet : IComponentData
    {
        public float lifeSpan;
        public float timer;
    }
}
