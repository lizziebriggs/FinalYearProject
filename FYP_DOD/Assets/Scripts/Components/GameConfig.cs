using Unity.Entities;
using UnityEngine;

namespace Components
{
    public struct GameConfig : IComponentData
    {
        public Entity enemyPrefab;
    }
}
