using SimpleSubScene.Components;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace SimpleSubScene.Systems
{
    [DisableAutoCreation]
    public class EntityCreationSystem : ComponentSystem
    {
        private EntityArchetype _aEntityData;
        private EntityQuery _qEntityCount;
        private EntityQuery _qEntityData;
        
        
        #region MAIN THREAD----------------------------------------------------

        protected override void OnCreate()
        {
            //
            // Enitity queries
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

            //
            // Set filters
            _qEntityCount.SetChangedVersionFilter(typeof(EntityCountComponent));
        }

        protected override void OnUpdate()
        {
            //
            // Create entities in component system
            Entities.With(_qEntityCount).ForEach((ref EntityCountComponent c) =>
            {
                //
                // Debug log
                Debug.Log("<b> <size=13> <color=#67A9DE>Info : 2 CreateSystem : Create Entities.</color> </size> </b>");

                //
                // Destroy old entities
                EntityManager.DestroyEntity(_qEntityData); //TODO : Could be done better

                //
                // Batch create new entities 
                var e = new NativeArray<Entity>(c.EntityCount, Allocator.TempJob);
                EntityManager.CreateEntity(_aEntityData, e);

                //
                // Cleanup
                e.Dispose();
            });
        }
        
        #endregion
    }
}