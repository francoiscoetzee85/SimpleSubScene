using SimpleSubScene.Components;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace SimpleSubScene.Systems
{
    public class SubSceneCreationSystem : ComponentSystem
    {
        private EntityArchetype _aEntityData;
        private EntityQuery _qEntityCount;
        private EntityQuery _qEntityData;

        protected override void OnCreate()
        {
            _aEntityData = EntityManager.CreateArchetype
            (
                ComponentType.ReadOnly<EntityDataComponent>()
            );

            _qEntityData = GetEntityQuery
            (
                typeof(EntityDataComponent)
            );

            _qEntityCount = GetEntityQuery
            (
                ComponentType.ReadOnly<EntityCountComponent>()
            );

            _qEntityCount.SetFilterChanged(typeof(EntityCountComponent));
        }

        protected override void OnUpdate()
        {
            Entities.With(_qEntityCount).ForEach((ref EntityCountComponent c) =>
            {
                Debug.Log("<b> <size=13> <color=#67A9DE>Info : 2 CreateSystem : Create Entities.</color> </size> </b>");

                EntityManager.DestroyEntity(_qEntityData); // NOT VERY HAPPY WHERE THIS IS SITTING

                var e = new NativeArray<Entity>(c.EntityCount, Allocator.TempJob);
                EntityManager.CreateEntity(_aEntityData, e);

                e.Dispose();
            });
        }
    }
}