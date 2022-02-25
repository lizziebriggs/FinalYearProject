using UnityEngine;

namespace Player
{
    public class FollowCamera : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Transform toFollow;
        [SerializeField] private Vector3 offset;
        [SerializeField] [Range(0f, 1f)] private float smoothing;
        
        private void LateUpdate()
        {
            if (!toFollow) return;
            
            var newPos = toFollow.position + offset;
            transform.position = Vector3.Slerp(transform.position, newPos, smoothing);
            transform.LookAt(toFollow);
        }
    }
}
