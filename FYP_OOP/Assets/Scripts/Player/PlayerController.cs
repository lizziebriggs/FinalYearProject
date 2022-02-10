using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float speed;
        private Vector3 dirToMove;
        private Vector3 dir;

        private void Start()
        {
            if (!rb) rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            var trans = rb.transform;
            
            //trans.Rotate(0, Input.GetAxis("Horizontal") * speed, 0);
            dirToMove.x = Input.GetAxis("Horizontal");
            dirToMove.z = Input.GetAxis("Vertical");
            dir = (trans.right * dirToMove.x) + (trans.forward * dirToMove.z);
            rb.MovePosition(trans.position + Time.deltaTime * speed * dir);
        }
    }
}
