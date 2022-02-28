using Components;
using Unity.Entities;

namespace Systems
{
    public class DamageSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((DynamicBuffer<CollisionBuffer> collision, ref Health health) => {
                for (int i = 0; i < collision.Length; i++)
                {
                    if (HasComponent<Damage>(collision[i].entity))
                    {
                        health.health -= GetComponent<Damage>(collision[i].entity).damage;
                    }
                }
            }).Schedule();
        }
    }
}
