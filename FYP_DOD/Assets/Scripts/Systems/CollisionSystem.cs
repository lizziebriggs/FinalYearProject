using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Systems
{
    [UpdateAfter(typeof(FixedStepSimulationSystemGroup))]
    public class CollisionSystem : SystemBase
    {
        private struct CollisionJob : ICollisionEventsJob
        {
            public BufferFromEntity<CollisionBuffer> collisions;
            
            public void Execute(CollisionEvent collisionEvent)
            {
                Entity entityA = collisionEvent.EntityA;
                Entity entityB = collisionEvent.EntityB;

                if (!entityA.Equals(Entity.Null))
                    collisions[entityA].Add(new CollisionBuffer {entity = entityB});
                if (!entityB.Equals(Entity.Null))
                    collisions[entityB].Add(new CollisionBuffer {entity = entityA});
            }
        }

        private struct TriggerJob : ITriggerEventsJob
        {
            public BufferFromEntity<TriggerBuffer> triggers;
            
            public void Execute(TriggerEvent triggerEvent)
            {
                Entity entityA = triggerEvent.EntityA;
                Entity entityB = triggerEvent.EntityB;

                if (entityA != Entity.Null)
                    triggers[entityA].Add(new TriggerBuffer {entity = entityB});
                if (entityA != Entity.Null)
                    triggers[entityB].Add(new TriggerBuffer {entity = entityA});
            }
        }
        
        protected override void OnUpdate()
        {
            var pw = World.GetOrCreateSystem<BuildPhysicsWorld>().PhysicsWorld;
            var sim = World.GetOrCreateSystem<StepPhysicsWorld>().Simulation;
            
            // Collisions

            // Refresh collisions each frame
            Entities.ForEach((DynamicBuffer<CollisionBuffer> collisions) =>
            {
                collisions.Clear();
            }).Run();

            var collisionJob = new CollisionJob
            {
                collisions = GetBufferFromEntity<CollisionBuffer>()
            }.Schedule(sim, ref pw, Dependency);
            
            collisionJob.Complete();

            // Triggers
            
            // Refresh triggers each frame
            Entities.ForEach((DynamicBuffer<TriggerBuffer> triggers) =>
            {
                triggers.Clear();
            }).Run();

            var triggerJob = new TriggerJob()
            {
                triggers = GetBufferFromEntity<TriggerBuffer>()
            }.Schedule(sim, ref pw, Dependency);
            
            triggerJob.Complete();
        }
    }
}
