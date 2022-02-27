using UnityEngine;

namespace Pickups
{
    public class SpeedPickup : Pickup
    {
        [Header("Speed Boost")]
        [SerializeField] private float speedMultiplier;
        [SerializeField] private float duration;

        private bool boostActive;
        private float timer;

        private void Update()
        {
            if (!boostActive) return;

            timer += Time.deltaTime;

            if (timer >= duration)
            {
                player.Speed /= speedMultiplier;
                gameObject.SetActive(false);
            }
        }

        protected override void OnPickUp()
        {
            base.OnPickUp();
            
            boostActive = true;
            player.Speed *= speedMultiplier;
        }
    }
}
