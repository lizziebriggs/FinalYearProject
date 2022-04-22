using System.Collections.Generic;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

namespace Enemies
{
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyController : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField] private float health;
        [SerializeField] private float damage;
        
        public float Health
        {
            get => health;
            set => health = value;
        }

        [Header("Movement")]
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float speed;
        private Vector3 dir;

        private void Start()
        {
            if (!rb) rb = GetComponent<Rigidbody>();
            
            dir = Vector3.forward;
        }

        private void Update()
        {
            if (health <= 0)
                Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            // Set layer mask so raycast ignores everything BUT walls and other enemies
            int layerMask = 1 << 6;
            bool hitWall = Physics.Raycast(transform.position, dir, out RaycastHit _, 2.5f, layerMask);
            
            if (hitWall)
            {
                var validDir = new List<Vector3>();
                
                // Cast raycast in each direction from entity and add to list of valid
                // directions if it does not hit a wall
                if(!CheckRay(Vector3.back))
                    validDir.Add(Vector3.back);
                if(!CheckRay(Vector3.forward))
                    validDir.Add(Vector3.forward);
                if(!CheckRay(Vector3.left))
                    validDir.Add(Vector3.left);
                if(!CheckRay(Vector3.right))
                    validDir.Add(Vector3.right);
                
                // If there are no valid directions, reverse the enemy movement
                if (validDir.Count == 0)
                    dir = -dir;
                else
                    dir = validDir[Random.Range(0, validDir.Count)];
            }

            rb.MovePosition(transform.position + Time.deltaTime * speed * dir);
        }

        private bool CheckRay(Vector3 dirToCheck)
        {
            // Returns true if the direction being checked is where the enemy just came from
            // or the raycast hit a wall or another enemy
            return dirToCheck == -dir
                   || Physics.Raycast(transform.position, dirToCheck, out RaycastHit _, 3f);
        }

        private void OnCollisionEnter(Collision other)
        {
            var player = other.gameObject.GetComponent<PlayerController>();

            if (player && !player.IsImmune)
            {
                player.Health -= damage;
                dir = -dir;
            }
        }
    }
}
