using Unity.Entities;

[GenerateAuthoringComponent]
public struct ShakeOnCollision : IComponentData
{
    public float Degree;
}
