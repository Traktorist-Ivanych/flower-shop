using DG.Tweening;
using FlowerShop.Flowers;
using FlowerShop.FlowersForCollection;
using FlowerShop.PickableObjects;
using FlowerShop.Saves.SaveData;
using FlowerShop.Settings;
using FlowerShop.Tables.Abstract;
using PlayerControl;
using Saves;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    /// <summary>
    /// Table, on which player can put flower for his own flowers collection
    /// </summary>
    public class FlowersForCollectionTable : Table, ISavableObject
    {
        [Inject] private readonly ActionsWithTransformSettings actionsWithTransformSettings;
        [Inject] private readonly FlowersForPlayerCollection flowersForCollection;
        [Inject] private readonly FlowersSettings flowersSettings;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly ReferencesForLoad referencesForLoad;

        [SerializeField] private Transform soilTablePosition;
        [SerializeField] private MeshRenderer soilMeshRenderer;
        [SerializeField] private MeshRenderer flowerMeshRenderer;

        [HideInInspector, SerializeField] private Transform soilTransform;
        [HideInInspector, SerializeField] private MeshFilter flowerMeshFilter;
        
        private Pot playerPot;
        private FlowerInfo flowerInfoForCollection;

        [field: SerializeField] public string UniqueKey { get; private set; }

        private void Awake()
        {
            Load();
        }

        private void OnValidate()
        {
            soilTransform = soilMeshRenderer.GetComponent<Transform>();
            flowerMeshFilter = flowerMeshRenderer.GetComponent<MeshFilter>();
        }

        private protected override void TryInteractWithTable()
        {
            base.TryInteractWithTable();

            if (CanPlayerPutFlowerOnTable())
            {
                SetPlayerDestinationAndOnPlayerArriveAction(PutFlowerOnTable);
            }
        }
        
        private protected override bool CanSelectedTableEffectBeDisplayed()
        {
            return CanPlayerPutFlowerOnTable();
        }

        public void Load()
        {
            FlowerInfoReferenceForSaving flowerInfoReferenceForLoading =
                SavesHandler.Load<FlowerInfoReferenceForSaving>(UniqueKey);

            if (flowerInfoReferenceForLoading.IsValuesSaved)
            {
                flowerInfoForCollection = referencesForLoad.GetReference<FlowerInfo>(
                        flowerInfoReferenceForLoading.FlowerInfoOnTableUniqueKey);

                if (flowerInfoForCollection != flowersSettings.FlowerInfoEmpty)
                {
                    SetFlowerOnTable();
                }
            }
        }

        public void Save()
        {
            FlowerInfoReferenceForSaving flowerInfoReferenceForSaving = new(flowerInfoForCollection.UniqueKey);
            
            SavesHandler.Save(UniqueKey, flowerInfoReferenceForSaving);
        }

        private bool CanPlayerPutFlowerOnTable()
        {
            if (playerBusyness.IsPlayerFree && flowerInfoForCollection == null && 
                playerPickableObjectHandler.CurrentPickableObject is Pot currentPot)
            {
                playerPot = currentPot;

                return playerPot.GrowingRoom == growingRoom &&
                       playerPot.FlowerGrowingLvl >= flowersSettings.MaxFlowerGrowingLvl &&
                       !playerPot.IsWeedInPot &&
                       flowersForCollection.IsFlowerForCollectionUnique(playerPot.PlantedFlowerInfo);
            }

            return false;
        }

        private void PutFlowerOnTable()
        {
            flowerInfoForCollection = playerPot.PlantedFlowerInfo;
            playerPot.CleanPot();

            SetFlowerOnTable();
            
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.ThrowTrigger);
            
            soilTransform.SetPositionAndRotation(
                position: playerPot.transform.position, 
                rotation: playerPot.transform.rotation);

            soilTransform.DOJump(
                    endValue: soilTablePosition.position,
                    jumpPower: actionsWithTransformSettings.PickableObjectDoTweenJumpPower,
                    numJumps: actionsWithTransformSettings.DefaultDoTweenJumpsNumber,
                    duration: actionsWithTransformSettings.MovingPickableObjectTime)
                .OnComplete(() => playerBusyness.SetPlayerFree());
            
            Save();
        }

        private void SetFlowerOnTable()
        {
            soilMeshRenderer.enabled = true;
            flowerMeshRenderer.enabled = true;
            flowerMeshFilter.mesh = flowerInfoForCollection.GetFlowerLvlMesh(flowersSettings.MaxFlowerGrowingLvl);
            
            flowersForCollection.AddFlowerToCollectionList(flowerInfoForCollection);
        }
    }
}
