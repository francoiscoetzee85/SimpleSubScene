using Unity.Entities;

namespace SimpleSubScene.Components
{
    public struct EntityCountComponent : IComponentData
    {
        public int EntityCount;
    }

    public struct EntityDataComponent : IComponentData
    {
        public int EntityData;
    }
}