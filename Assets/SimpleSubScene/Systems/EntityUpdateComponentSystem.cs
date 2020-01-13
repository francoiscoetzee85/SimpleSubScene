using System.Collections.Generic;
using SimpleSubScene.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace SimpleSubScene.Systems
{
    //[DisableAutoCreation]
    [UpdateAfter(typeof(EntityCreationSystem))]
    public class EntityUpdateComponentSystem : JobComponentSystem
    {
        private EntityQuery _qEntityData;
        
        protected override void OnCreate()
        {
            //
            // Entity queries 
            _qEntityData = GetEntityQuery
            (
                typeof(EntityDataComponent)
            );

            //
           // Set filters
            _qEntityData.SetChangedVersionFilter(typeof(EntityDataComponent));
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {

            #region ENTITIES 0.4.0 -----------------------------------------------
            
            // TODO : investigate passing EntityQuery to Schedule()
            
            //
            // API 0.4.0 updating inputDeps = new SetData().Schedule(_qEntityData, inputDeps);
            inputDeps = Entities.ForEach((ref EntityDataComponent c) => { c.EntityData = 11; }).Schedule(inputDeps);
            inputDeps.Complete();
            
            #endregion

            #region FILTER BY SCAREDCOMPONENT DATA VALUE -------------------------

            //
            // Update by SCD 
            // Running a Job by filtering a shared component value
            
            //
            // Get all the chunks with unique shared component values
            List<EntityIdComponent> ids = new List<EntityIdComponent>();
            EntityManager.GetAllUniqueSharedComponentData<EntityIdComponent>(ids);
            // TODO : NativeList<JobHandle> dependencies = new NativeList<JobHandle>( Allocator.Temp);

            //
            // Iterate over all unique shared component values 
            // If we have 200 entities and 100 og the has a SCD value of 1
            // and the other has a SCD of 5 we will have 2 unique SCD values
            // and id length would be 2

            foreach (EntityIdComponent id in ids)
            {
                //
                // Schedule a job based on SCD value match
                
                if (id.EntityId == 1)
                {
                    JobHandle thisJobHandle
                        = Entities.WithSharedComponentFilter(id)
                            .ForEach((ref EntityDataComponent data) => { data.EntityData = 111; })
                            .Schedule(inputDeps);
                    
                    thisJobHandle.Complete();
                    // TODO : dependencies.Add(thisJobHandle);
                }
                
                if (id.EntityId == 2)
                {
                    JobHandle thisJobHandle
                        = Entities.WithSharedComponentFilter(id)
                            .ForEach((ref EntityDataComponent data) => { data.EntityData = 222; })
                            .Schedule(inputDeps);

                    thisJobHandle.Complete();
                    // TODO : dependencies.Add(thisJobHandle);
                }
            }
            
            // TODO : EntityManager.DestroyEntity(_qEntityData);
            return inputDeps; // TODO : //CombineDependencies(dependencies);;

            #endregion
        }
    }
}