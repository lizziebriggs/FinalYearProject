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

        protected override void Update()
        {
            base.Update();
            
            if (!boostActive) return;

            timer += Time.deltaTime;

            // Remove speed boost effect once timer ends
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
