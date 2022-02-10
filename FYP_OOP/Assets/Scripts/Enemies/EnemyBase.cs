using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Rigidbody), typeof(Material))]
    public class EnemyBase : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField] protected Color colour;
        [SerializeField] protected Material mat;

        [Header("Movement")]
        [SerializeField] protected Rigidbody rb;
        [SerializeField] protected float speed;

        private void Start()
        {
            if (!rb) rb = GetComponent<Rigidbody>();
            if (!mat) mat = GetComponent<Material>();

            mat.color = colour;
        }
    }
}
