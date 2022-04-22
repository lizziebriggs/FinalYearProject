using Components;
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

                // If collision occurs, add other to entity to collision buffer
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

                // If collision occurs, add other to entity to trigger buffer
                if (!entityA.Equals(Entity.Null))
                    triggers[entityA].Add(new TriggerBuffer {entity = entityB});
                if (!entityB.Equals(Entity.Null))
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

            // Run collision job
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

            // Run trigger job
            var triggerJob = new TriggerJob
            {
                triggers = GetBufferFromEntity<TriggerBuffer>()
            }.Schedule(sim, ref pw, Dependency);
            
            triggerJob.Complete();
        }
    }
}
