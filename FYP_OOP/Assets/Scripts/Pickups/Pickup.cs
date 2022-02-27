using Player;
using UnityEngine;

namespace Pickups
{
    [RequireComponent(typeof(Material), typeof(MeshRenderer), typeof(Collider))]
    public class Pickup : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRend;
        [SerializeField] private Collider col;

        protected PlayerController player;

        private void Start()
        {
            if (!meshRend) meshRend = GetComponent<MeshRenderer>();
            if (!col) col = GetComponent<Collider>();
        }

        protected virtual void OnPickUp()
        {
            meshRend.enabled = false;
            col.enabled = false;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            player = other.gameObject.GetComponent<PlayerController>();
            
            if (player)
                OnPickUp();
        }
    }
}
