using System;
using Unity.Entities;

namespace SimpleSubScene.Components
{
    public struct EntityIdComponent : ISharedComponentData
    {
        public int EntityId;
    }
}