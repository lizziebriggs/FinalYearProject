using Unity.Entities;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct Health : IComponentData
    {
        public int health;
    }
}
