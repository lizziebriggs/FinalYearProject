using System;
using Unity.Entities;

namespace Components
{
    public struct GameConfig : IComponentData
    {
        public int numOfEnemies;
    }
}
