using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
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
        private Vector3 dirToMove;
        private Vector3 dir;

        public float Speed
        {
            get => speed;
            set => speed = value;
        }
        
        [Header("Bullets")]
        [SerializeField] private GameObject bullet;
        [SerializeField] private float fireRate;
        [SerializeField] private int poolSize;
        private float fireTimer;
        private List<GameObject> bulletPool = new List<GameObject>();
        private int nextBullet;

        
        private void Start()
        {
            if (!rb) rb = GetComponent<Rigidbody>();

            health = maxHealth;

            for (int i = 0; i < poolSize; i++)
            {
                var newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                newBullet.SetActive(false);
                bulletPool.Add(newBullet);
            }

            nextBullet = 0;
        }

        private void OnGUI()
        {
            GUI.Box(new Rect(10, 10, 100, 30), "Health: " + health);
        }

        private void Update()
        {
            if (health <= 0)
            {
                // End game
            }

            if (fireTimer >= fireRate)
            {
                if (Input.GetMouseButton(0))
                {
                    Fire();
                    fireTimer = 0;
                }
            }
            else fireTimer += Time.deltaTime;
        }

        private void FixedUpdate()
        {
            var trans = rb.transform;
            
            dirToMove.x = Input.GetAxis("Horizontal");
            dirToMove.z = Input.GetAxis("Vertical");
            dir = (trans.right * dirToMove.x) + (trans.forward * dirToMove.z);
            rb.MovePosition(trans.position + Time.deltaTime * speed * dir);
        }

        private void Fire()
        {
            bulletPool[nextBullet].transform.position = transform.position;
            bulletPool[nextBullet].SetActive(true);

            nextBullet = nextBullet + 1 == poolSize ? 0 : ++nextBullet;
        }
    }
}
