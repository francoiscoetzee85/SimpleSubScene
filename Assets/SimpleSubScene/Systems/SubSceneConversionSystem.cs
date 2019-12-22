using SimpleSubScene.Components;
using SimpleSubScene.Proxies;
using Unity.Entities;
using UnityEngine;

namespace SimpleSubScene.Systems
{
    public class SubSceneConversionSystem : GameObjectConversionSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((SubSceneProxy data) =>
            {
                Debug.Log("<b> <size=13> <color=#DE67DA>Info : 1 SubSceneConversionSystem : Converting Entities.</color> </size> </b>");

                Entity ePrimary = GetPrimaryEntity(data);
                var cData = new EntityCountComponent
                {
                    EntityCount = data.numEntities
                };
                DstEntityManager.AddComponentData(ePrimary, cData);
            });
        }
    }
}