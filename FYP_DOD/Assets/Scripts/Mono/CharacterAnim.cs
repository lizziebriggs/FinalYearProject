using Components;
using UnityEngine;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using Unity.Mathematics;

namespace Mono
{
    public class CharacterAnim : MonoBehaviour
    {
        [SerializeField] private Animator anim;
        [SerializeField] Transform trans;
        
        private void Update()
        {
            var em = World.DefaultGameObjectInjectionWorld.EntityManager;
            var playerQuery = em.CreateEntityQuery(typeof(Player), typeof(Movable),
                typeof(Translation), typeof(Rotation));

            if (playerQuery.CalculateEntityCount() > 0)
            {
                var playerEntity = playerQuery.GetSingletonEntity();
                trans.position = em.GetComponentData<Translation>(playerEntity).Value;

                var dir = em.GetComponentData<Movable>(playerEntity).direction;
                var speed = !Equals(dir, float3.zero) ? 1 : 0;
                
                if (math.length(dir) > .2f)
                    trans.rotation = Quaternion.LookRotation(dir);
                
                anim.SetFloat("Speed", speed * 3f);
            }
        }
    }
}
