using Enemies;
using UnityEngine;

namespace Player
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float damage;
        [SerializeField] private float speed;
        [SerializeField] private float lifeSpan;
        private float timer;


        private void FixedUpdate()
        {
            transform.position += transform.forward * (Time.deltaTime * speed);

            // Destroy bullet after life span runs out
            timer += Time.deltaTime;
            if (timer >= lifeSpan)
                Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Wall"))
                Destroy(gameObject);
            
            var enemy = other.gameObject.GetComponent<EnemyController>();

            if (enemy)
            {
                enemy.Health -= damage;
                Destroy(gameObject);
            }
        }
    }
}
