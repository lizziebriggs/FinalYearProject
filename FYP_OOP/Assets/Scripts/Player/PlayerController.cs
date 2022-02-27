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

        public float Speed
        {
            get => speed;
            set => speed = value;
        }

        private bool isImmune;
        public bool IsImmune
        {
            get => isImmune;
            set => isImmune = value;
        }

        private void Start()
        {
            if (!rb) rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            var trans = rb.transform;
            
            dirToMove.x = Input.GetAxis("Horizontal");
            dirToMove.z = Input.GetAxis("Vertical");
            dir = (trans.right * dirToMove.x) + (trans.forward * dirToMove.z);
            rb.MovePosition(trans.position + Time.deltaTime * speed * dir);
        }
    }
}
