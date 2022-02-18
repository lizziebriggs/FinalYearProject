using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Mono
{
    public class FollowEntity : MonoBehaviour
    {
        [SerializeField] private Entity entityToFollow;
        [SerializeField] private float3 offset;

        public Entity EntityToFollow
        {
            get => entityToFollow;
            set => entityToFollow = value;
        }

        private EntityManager entityManager;

        private void Awake()
        {
            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        }

        private void LateUpdate()
        {
            if (entityManager == null) return;

            Translation entityPos = entityManager.GetComponentData<Translation>(entityToFollow);
            transform.position = entityPos.Value + offset;
        }
    }
}
