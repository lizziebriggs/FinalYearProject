using Unity.Entities;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct Maze : IComponentData
    {
        public int width, length;
        public float pathWidth, pathHeight;
        public Entity floorQuad, wallQuad;

        public float startX, startY;
        public float endX, endY;
    }
}
