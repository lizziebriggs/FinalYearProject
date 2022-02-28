using System;
using Unity.Entities;

namespace Components
{
    public struct TriggerBuffer : IBufferElementData
    {
        public Entity entity;
    }
}
