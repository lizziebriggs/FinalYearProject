using UnityEngine;

namespace Pickups
{
    public class ImmunityPickup : Pickup
    {
        [Header("Immunity")]
        [SerializeField] private float duration;

        private bool immunityActive;
        private float timer;

        private void Update()
        {
            if (!immunityActive) return;

            timer += Time.deltaTime;

            if (timer >= duration)
            {
                gameObject.SetActive(false);
                player.IsImmune = false;
            }
        }

        protected override void OnPickUp()
        {
            base.OnPickUp();
            
            immunityActive = true;
            player.IsImmune = true;
        }
    }
}
