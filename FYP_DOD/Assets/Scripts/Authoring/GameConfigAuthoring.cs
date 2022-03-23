using System.Collections.Generic;
using Components;
using Unity.Entities;
using UnityEngine;
using UnityGameObject = UnityEngine.GameObject;

namespace Authoring
{
    public class GameConfigAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    {
        public UnityGameObject enemyPrefab;

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.Add(enemyPrefab);
        }
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var gameConfig = new GameConfig()
            {
                enemyPrefab = conversionSystem.GetPrimaryEntity(enemyPrefab)
            };

            dstManager.AddComponentData(entity, gameConfig);
        }
    }
}
