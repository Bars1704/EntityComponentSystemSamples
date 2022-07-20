using System.Linq;
using Shouldly;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;


[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(ExportPhysicsWorld)), UpdateBefore(typeof(EndFramePhysicsSystem))]
public partial class DebugCollisionSystem : SystemBase
{
    StepPhysicsWorld m_StepPhysicsWorldSystem;
    private BuildPhysicsWorld PhysicsWorldSystem;
    EntityQuery m_ShakeGroup;

    protected override void OnCreate()
    {
        m_StepPhysicsWorldSystem = World.GetOrCreateSystem<StepPhysicsWorld>();
        PhysicsWorldSystem = World.GetExistingSystem<BuildPhysicsWorld>();

        m_ShakeGroup = GetEntityQuery(new EntityQueryDesc
        {
            All = new ComponentType[]
            {
                typeof(ShakeOnCollision)
            }
        });
    }

    [BurstCompile]
    struct CollisionEventImpulseJob : ICollisionEventsJob
    {
        [ReadOnly] public PhysicsWorld PhysicsWorld;

        [ReadOnly] public ComponentDataFromEntity<ShakeOnCollision> ShakeOnCollisionGroup;
        public ComponentDataFromEntity<PhysicsJoint> JointGroup;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;


            foreach (var x in PhysicsWorld.Joints)
            {
            }


            // var x = PhysicsWorld.DynamicsWorld.GetJointIndex(entityA);
            // Debug.Log($"X:{x}");


            // var y = PhysicsWorld.DynamicsWorld.GetJointIndex(entityB);
            // Debug.Log($"Y:{y}");
        }
    }

    protected override void OnStartRunning()
    {
        base.OnStartRunning();
        this.RegisterPhysicsRuntimeSystemReadOnly();
    }

    protected override void OnUpdate()
    {
        if (m_ShakeGroup.CalculateEntityCount() == 0)
        {
            return;
        }

        Dependency = new CollisionEventImpulseJob
        {
            ShakeOnCollisionGroup = GetComponentDataFromEntity<ShakeOnCollision>(true),
            JointGroup = GetComponentDataFromEntity<PhysicsJoint>(),
            PhysicsWorld = PhysicsWorldSystem.PhysicsWorld,
        }.Schedule(m_StepPhysicsWorldSystem.Simulation, Dependency);
    }
}
