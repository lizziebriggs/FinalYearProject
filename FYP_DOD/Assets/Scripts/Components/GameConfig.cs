using Unity.Entities;

namespace Components
{
    public struct GameConfig : IComponentData
    {
        public Entity enemyPrefab;
    }
}
