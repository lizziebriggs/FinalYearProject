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
                gameObject.SetActive(false);
        }

        private void OnCollisionEnter(Collision other)
        {
            var enemy = other.gameObject.GetComponent<EnemyBase>();

            if (enemy)
            {
                enemy.Health -= damage;
                gameObject.SetActive(false);
            }
        }
    }
}
