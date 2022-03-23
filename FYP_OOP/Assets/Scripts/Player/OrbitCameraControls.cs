using UnityEngine;

namespace Player
{
    public class OrbitCameraControls : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Transform toFollow;
        [SerializeField] private float rotateSpeed;
        [SerializeField] [Range(0f, 1f)] private float smoothing;

        private Vector3 posOffset;

        private void Start()
        {
            posOffset = transform.position - toFollow.position;
        }

        private void LateUpdate()
        {
            Quaternion turnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotateSpeed, Vector3.up);
            posOffset = turnAngle * posOffset;

            var trans = transform;
            Vector3 newPos = toFollow.position + posOffset;
            
            trans.position = Vector3.Slerp(trans.position, newPos, smoothing);
            trans.LookAt(toFollow);

            var followTrans = toFollow.transform;
            var followEuler = followTrans.localEulerAngles;
            followTrans.localEulerAngles = new Vector3(followEuler.x, trans.localEulerAngles.y, followEuler.z);
        }
    }
}
