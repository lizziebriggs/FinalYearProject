using System.Collections.Generic;
using Components;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class GameConfigAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    {
        [Header("Enemies")]
        public int numOfEnemies;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var gameConfig = new GameConfig()
            {
                numOfEnemies = numOfEnemies
            };
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            
        }
    }
}
