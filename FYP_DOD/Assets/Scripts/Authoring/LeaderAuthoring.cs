using Mono;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    [RequiresEntityConversion]
    [AddComponentMenu("Custom Authoring/Leader Authoring")]
    public class LeaderAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] private FollowEntity followEntity;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            if (!followEntity)
                followEntity = followEntity.gameObject.AddComponent<FollowEntity>();

            followEntity.EntityToFollow = entity;
        }
    }
}
