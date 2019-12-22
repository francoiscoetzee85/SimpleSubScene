using SimpleSubScene.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace SimpleSubScene.Systems
{
    [UpdateAfter(typeof(SubSceneCreationSystem))]
    public class SubSceneUpdateComponentSystem : JobComponentSystem
    {
        private EntityQuery _qEntityData;

        protected override void OnCreate()
        {
            _qEntityData = GetEntityQuery
            (
                typeof(EntityDataComponent)
            );

            _qEntityData.SetFilterChanged(typeof(EntityDataComponent));
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            inputDeps = new SetData().Schedule(_qEntityData, inputDeps);
        
            inputDeps.Complete(); 
                           
            EntityManager.DestroyEntity(_qEntityData);
                    
            return inputDeps;
        }

        [BurstCompile]
        private struct SetData : IJobForEachWithEntity<EntityDataComponent>
        {
            public void Execute(Entity entity, int index, ref EntityDataComponent c)
            {
                // WARNING : This does not print to console for some reason but the
                // job is working.
                DebugInfo();

                c.EntityData = index;
            }

            [BurstDiscard]
            private void DebugInfo()
            {
                Debug.Log("<b> <size=13> <color=#9DF155>Info : 3 SetDataSystem : Setting Data .</color> </size> </b>");
            }
        }
    }
}