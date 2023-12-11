using DG.Tweening;
using FlowerShop.Flowers;
using FlowerShop.PickableObjects;
using FlowerShop.Settings;
using FlowerShop.Tables.Abstract;
using PlayerControl;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    /// <summary>
    /// Table, on which player can put flower for his own flower collection
    /// </summary>
    public class FlowersForCollectionTable : Table
    {
        [Inject] private readonly FlowersSettings flowersSettings;
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;
        [Inject] private readonly PlayerComponents playerComponents;

        [SerializeField] private Transform soilTablePosition;
        [SerializeField] private MeshRenderer soilMeshRenderer;
        [SerializeField] private MeshRenderer flowerMeshRenderer;
        [SerializeField] private FlowersForCollection.FlowersForCollection flowersForCollection;

        [HideInInspector, SerializeField] private Transform soilTransform;
        [HideInInspector, SerializeField] private MeshFilter flowerMeshFilter;
        
        private Pot playerPot;
        private FlowerInfo flowerInfoForCollection;

        private void OnValidate()
        {
            soilTransform = soilMeshRenderer.GetComponent<Transform>();
            flowerMeshFilter = flowerMeshRenderer.GetComponent<MeshFilter>();
        }

        public override void ExecuteClickableAbility()
        {
            if (playerPickableObjectHandler.CurrentPickableObject is Pot currentPot)
            {
                playerPot = currentPot;

                if (CanFlowerBePuttedOnFlowerTableForCollection())
                {
                    SetPlayerDestination();
                }
            }
        }

        public override void ExecutePlayerAbility()
        {
            soilMeshRenderer.enabled = true;
            flowerMeshRenderer.enabled = true;
            flowerInfoForCollection = playerPot.PlantedFlowerInfo;
            playerPot.CleanPot();
            flowerMeshFilter.mesh = flowerInfoForCollection.GetFlowerLvlMesh(flowersSettings.MaxFlowerGrowingLvl);
            
            soilTransform.SetPositionAndRotation(
                position: playerPot.transform.position, 
                rotation: playerPot.transform.rotation);

            soilTransform.DOJump(
                    endValue: soilTablePosition.position,
                    jumpPower: actionsWithTransformSettings.PickableObjectDoTweenJumpPower,
                    numJumps: actionsWithTransformSettings.DefaultDoTweenJumpsNumber,
                    duration: actionsWithTransformSettings.MovingPickableObjectTime)
                .OnComplete(() => playerBusyness.SetPlayerFree());
            
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.ThrowTrigger);
            
            flowersForCollection.AddFlowerToCollectionList(flowerInfoForCollection);
        }

        private bool CanFlowerBePuttedOnFlowerTableForCollection()
        {
            return playerBusyness.IsPlayerFree && 
                   playerPot.GrowingRoom == growingRoom &&
                   playerPot.FlowerGrowingLvl >= flowersSettings.MaxFlowerGrowingLvl &&
                   !playerPot.IsWeedInPot &&
                   flowersForCollection.IsFlowerForCollectionUnique(playerPot.PlantedFlowerInfo) && 
                   flowerInfoForCollection == null;
        }
    }
}
