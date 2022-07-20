using Unity.Entities;

namespace Unity.Physics.Authoring
{
    public static class JointLinker
    {
        public static void AddJointLink(PhysicsConstrainedBodyPair bodyPair, Entity jointEntity)
        {
            var manager = World.DefaultGameObjectInjectionWorld.EntityManager;

            manager.AddBuffer<JointLinkBuffer>(bodyPair.EntityA).Add(jointEntity);
            manager.AddBuffer<JointLinkBuffer>(bodyPair.EntityB).Add(jointEntity);
        }
    }

    [InternalBufferCapacity(1)]
    public struct JointLinkBuffer : IBufferElementData
    {
        public static implicit operator Entity(JointLinkBuffer e) { return e.Value; }
        public static implicit operator JointLinkBuffer(Entity e) { return new JointLinkBuffer { Value = e }; }

        public Entity Value;
    }
}
