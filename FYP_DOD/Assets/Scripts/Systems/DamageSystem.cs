using Components;
using Unity.Entities;

namespace Systems
{
    public class DamageSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var dt = Time.DeltaTime;
            
            Entities.ForEach((DynamicBuffer<CollisionBuffer> collision, ref Health health) => {
                for (int i = 0; i < collision.Length; i++)
                {
                    if (health.hitDelay <= 0 && !health.isImmune && HasComponent<Damage>(collision[i].entity))
                    {
                        health.health -= GetComponent<Damage>(collision[i].entity).damage;
                    }
                }
            }).Schedule();

            // Tick hit delay timer
            Entities.ForEach((Entity e, ref Health health) =>
            {
                health.hitDelay -= dt;

                if (health.health <= 0)
                {
                    // end game
                }
            }).Schedule();
        }
    }
}
