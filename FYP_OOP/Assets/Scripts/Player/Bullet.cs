using UnityEngine;

namespace Player
{
    public class Bullet : MonoBehaviour
    {
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
    }
}
