using SimpleSubScene.Components;
using SimpleSubScene.Proxies;
using Unity.Entities;
using UnityEngine;

namespace SimpleSubScene.Systems
{
    public class EntityConversionSystem : GameObjectConversionSystem
    {
        #region MAIN THREAD----------------------------------------------------
        protected override void OnUpdate()
        {
            Entities.ForEach((EntityProxy data) =>
            {
                //
                // Debug log
                Debug.Log("<b> <size=13> <color=#DE67DA>Info : 1 SubSceneConversionSystem : Converting Entities.</color> </size> </b>");
                
                //
                // Get primary entity
                Entity ePrimary = GetPrimaryEntity(data);
                
                //
                // Add component data
                var cData = new EntityCountComponent
                {
                    EntityCount = data.entityCount
                };
                DstEntityManager.AddComponentData(ePrimary, cData);
                
                //
                // Add share component data
                DstEntityManager.AddSharedComponentData(ePrimary, new EntityIdComponent {EntityId = data.entityId});
                
                //
                // Add additional entity
                for (var i = 0; i < data.entityCount; i++)
                {
                    Entity e = CreateAdditionalEntity(data);
                    
                    //
                    // Add components
                    DstEntityManager.AddSharedComponentData(e, new EntityIdComponent {EntityId = data.entityId});
                    DstEntityManager.AddComponentData(e, new EntityDataComponent() {EntityData = data.entityData});
                }
            });
        }
        
        #endregion
    }
}