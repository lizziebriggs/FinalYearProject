using System;
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
                    Fire();
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
            
            transform.Translate(movement * (speed * Time.deltaTime), Space.World);
            if (movement != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15F);
        }

        private void Fire()
        {
            bulletPool[nextBullet].transform.position = transform.position;
            bulletPool[nextBullet].transform.rotation = transform.rotation;
            bulletPool[nextBullet].SetActive(true);

            nextBullet = nextBullet + 1 == poolSize ? 0 : ++nextBullet;
        }
    }
}
