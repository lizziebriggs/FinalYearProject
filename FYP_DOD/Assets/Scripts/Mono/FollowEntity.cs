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
        [SerializeField] private bool enableZoom;
        [SerializeField] private float zoomSpeed;

        public Entity EntityToFollow
        {
            set => entityToFollow = value;
        }

        private EntityManager entityManager;
        

        private float originalOffset;

        private void Start()
        {
            originalOffset = offset.y;
        }

        private void Awake()
        {
            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        }

        private void LateUpdate()
        {
            if (entityManager == null) return;

            // Match position of the camera to the player's
            Translation entityPos = entityManager.GetComponentData<Translation>(entityToFollow);
            transform.position = entityPos.Value + offset;
            
            // Mouse input camera zoom
            if(enableZoom)
                Zoom(Input.GetAxis("Mouse ScrollWheel"));

            if (Input.GetKey(KeyCode.F))
                offset.y = originalOffset;
        }

        private void Zoom(float zoom)
        {
            // Zoom in
            if (zoom > 0 && offset.y >= originalOffset)
                offset.y -= zoomSpeed;
            
            // Zoom out
            else if (zoom < 0)
                offset.y += zoomSpeed;
        }
    }
}
