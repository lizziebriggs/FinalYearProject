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

            timer += Time.deltaTime;
            if (timer >= lifeSpan)
                Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Wall"))
                Destroy(gameObject);
            
            var enemy = other.gameObject.GetComponent<EnemyBase>();

            if (enemy)
            {
                enemy.Health -= damage;
                Destroy(gameObject);
            }
        }
    }
}
