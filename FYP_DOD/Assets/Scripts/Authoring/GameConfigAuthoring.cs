using System.Collections.Generic;
using Components;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class GameConfigAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    {
        [Header("Enemies")]
        public int enemyFrequency;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var gameConfig = new GameConfig()
            {
                enemyFrequency = enemyFrequency
            };
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            
        }
    }
}
