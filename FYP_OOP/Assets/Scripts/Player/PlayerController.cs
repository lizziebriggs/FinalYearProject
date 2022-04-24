using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Animator anim;
        
        [Header("Settings")]
        [SerializeField] private float maxHealth;
        private float health;

        public float Health
        {
            get => health;
            set => health = value;
        }

        public bool IsImmune { get; set; }
        
        [Header("Movement")]
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float speed;

        public float Speed
        {
            get => speed;
            set => speed = value;
        }
        
        [Header("Bullets")]
        [SerializeField] private GameObject bullet;
        [SerializeField] private float fireRate;
        private float fireTimer;

        
        private void Start()
        {
            if (!rb) rb = GetComponent<Rigidbody>();

            health = maxHealth;
        }

        private void OnGUI()
        {
            GUI.Box(new Rect(10, 10, 100, 25), "Health: " + health);
        }

        public void Reset()
        {
            health = maxHealth;
        }

        private void Update()
        {
            if (fireTimer >= fireRate)
            {
                if (Input.GetMouseButton(0))
                {
                    Instantiate(bullet, transform.position, transform.rotation);
                    fireTimer = 0;
                }
            }
            else fireTimer += Time.deltaTime;
        }

        private void FixedUpdate()
        {
            float xMove = Input.GetAxis("Horizontal");
            float zMove = Input.GetAxis("Vertical");
            
            Vector3 movement = new Vector3(xMove, 0, zMove);
            movement.Normalize();
            
            // Move player based on user input
            transform.Translate(movement * (speed * Time.deltaTime), Space.World);
            if (movement != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15F);
            
            // Match animation to player's movement
            var animSpeed = !movement.Equals(Vector3.zero) ? 1 : 0;
            anim.SetFloat("Speed", animSpeed * 3f);
        }
    }
}
