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
        
        [Header("Info")]
        [SerializeField] private float maxHealth;
        private float health;

        public float Health
        {
            get => health;
            set => health = value;
        }

        public bool IsImmune { get; set; }

        private void Start()
        {
            if (!rb) rb = GetComponent<Rigidbody>();

            health = maxHealth;
        }

        private void Update()
        {
            if (health <= 0)
            {
                // End game
            }
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
