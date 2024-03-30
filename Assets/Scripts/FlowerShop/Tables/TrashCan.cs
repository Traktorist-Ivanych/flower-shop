using FlowerShop.Flowers;
using FlowerShop.PickableObjects;
using FlowerShop.Sounds;
using FlowerShop.Tables.Abstract;
using PlayerControl;
using UnityEngine;
using Zenject;

namespace FlowerShop.Tables
{
    /// <summary>
    /// Table, allowing Player to empty current pot content
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class TrashCan : Table
    {
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly FlowersSettings flowersSettings;
        [Inject] private readonly SoundsHandler soundsHandler;

        [SerializeField] private MeshRenderer flowerRenderer;
        [SerializeField] private MeshRenderer soilRenderer;
        [SerializeField] private MeshRenderer weedRenderer;

        [HideInInspector, SerializeField] private Animator trashCanAnimator;
        [HideInInspector, SerializeField] private MeshFilter flowerMeshFilter;
        [HideInInspector, SerializeField] private MeshFilter weedMeshFilter;
    
        private Pot playerPot;
        private static readonly int Throw = Animator.StringToHash("Throw");

        private void OnValidate()
        {
            trashCanAnimator = GetComponent<Animator>();
            flowerMeshFilter = flowerRenderer.GetComponent<MeshFilter>();
            weedMeshFilter = weedRenderer.GetComponent<MeshFilter>();
        }

        private protected override void TryInteractWithTable()
        {
            base.TryInteractWithTable();

            if (CanPlayerThrowOutPotContent())
            {
                SetPlayerDestinationAndOnPlayerArriveAction(ThrowOutPotContent);
            }
        }
        
        private protected override bool CanSelectedTableEffectBeDisplayed()
        {
            return CanPlayerThrowOutPotContent();
        }

        private void PlayOpenTrashCanAudio()
        {
            soundsHandler.PlayOpenTrashCanAudio();
        }
        
        private void PlayCloseTrashCanAudio()
        {
            soundsHandler.PlayCloseTrashCanAudio();
        }

        private bool CanPlayerThrowOutPotContent()
        {
            if (playerBusyness.IsPlayerFree && playerPickableObjectHandler.CurrentPickableObject is Pot currentPot)
            {
                playerPot = currentPot;

                return playerPot.IsSoilInsidePot;
            }

            return false;
        }

        private void ThrowOutPotContent()
        {
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.ThrowTrigger);
        
            soilRenderer.enabled = true;
        
            if (playerPot.PlantedFlowerInfo.FlowerName != flowersSettings.FlowerNameEmpty)
            {
                flowerRenderer.enabled = true;
                flowerMeshFilter.mesh = playerPot.PotObjects.FlowerMeshFilter.mesh;
            }
        
            if (playerPot.IsWeedInPot)
            {
                weedRenderer.enabled = true;
                weedMeshFilter.mesh = playerPot.PotObjects.WeedMeshFilter.mesh;
            }

            playerPot.CleanPot();
        
            trashCanAnimator.SetTrigger(Throw);
        }

        private void FinishThrowingOutPotContent()
        {
            flowerRenderer.enabled = false;
            soilRenderer.enabled = false;
            weedRenderer.enabled = false;
            playerBusyness.SetPlayerFree();
        }
    }
}
