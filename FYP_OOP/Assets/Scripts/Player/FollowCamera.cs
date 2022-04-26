using UnityEngine;

namespace Player
{
    public class FollowCamera : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Transform toFollow;
        [SerializeField] private Vector3 offset;
        [SerializeField] private bool enableZoom;
        [SerializeField] private float zoomSpeed;

        private float originalOffset;

        private void Start()
        {
            originalOffset = offset.y;
        }

        private void LateUpdate()
        {
            if (!toFollow) return;
            
            // Match position to player's position
            var newPos = toFollow.position + offset;
            transform.position = newPos;
            transform.LookAt(toFollow);
            
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
