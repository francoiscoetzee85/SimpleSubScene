using System;
using Unity.Entities;

namespace SimpleSubScene.Components
{
    public struct EntityCountComponent : IComponentData
    {
        public int EntityCount;
    }
}