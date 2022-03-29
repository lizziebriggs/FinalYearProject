using Unity.Entities;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct Maze : IComponentData
    {
        public int width, length;
        public float pathWidth, pathHeight;
        public Entity floor, wall, water;

        public Entity enemyPrefab;
        public float enemyChance;

        public Entity speedPickupPrefab;
        public Entity immunityPickupPrefab;
        public float pickupChance;

        public float startX, startY;
        public float endX, endY;
    }
}
