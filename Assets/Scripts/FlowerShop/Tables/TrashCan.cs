using FlowerShop.Achievements;
using FlowerShop.Flowers;
using FlowerShop.Help;
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
        [Inject] private readonly DecentCitizen decentCitizen;
        [Inject] private readonly FlowersSettings flowersSettings;
        [Inject] private readonly HelpCanvasLiaison helpCanvasLiaison;
        [Inject] private readonly HelpTexts helpTexts;
        [Inject] private readonly PlayerComponents playerComponents;
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
            else if (CanPlayerUseTableInfoCanvas())
            {
                SetPlayerDestinationAndOnPlayerArriveAction(UseTableInfoCanvas);
            }
            else
            {
                TryToShowHelpCanvas();
            }
        }
        private void TryToShowHelpCanvas()
        {
            if (!playerBusyness.IsPlayerFree)
            {
                helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.PlayerBusy);
            }
            else if (playerPickableObjectHandler.CurrentPickableObject is Pot currentPot)
            {
                if (currentPot.GrowingRoom != growingRoom)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.MismatchGrowingRoom);
                }
                else if (!currentPot.IsSoilInsidePot)
                {
                    helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.PotEmpty);
                }
            }
            else if (playerPickableObjectHandler.IsPickableObjectNull)
            {
                helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.EmptyHands);
            }
            else
            {
                helpCanvasLiaison.EnableCanvasAndSetHelpText(helpTexts.WrongPickableObject);
            }
        }


        private protected override bool CanSelectedTableEffectBeDisplayed()
        {
            return CanPlayerThrowOutPotContent() || CanPlayerUseTableInfoCanvas();
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

                return playerPot.GrowingRoom == growingRoom && playerPot.IsSoilInsidePot;
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
            decentCitizen.IncreaseProgress();
        
            trashCanAnimator.SetTrigger(Throw);
        }

        private void FinishThrowingOutPotContent()
        {
            flowerRenderer.enabled = false;
            soilRenderer.enabled = false;
            weedRenderer.enabled = false;
            playerBusyness.SetPlayerFree();
        }

        private bool CanPlayerUseTableInfoCanvas()
        {
            return playerBusyness.IsPlayerFree && playerPickableObjectHandler.CurrentPickableObject is InfoBook;
        }

        private void UseTableInfoCanvas()
        {
            tableInfoCanvasLiaison.ShowCanvas(tableInfo, growingRoom);
        }
    }
}
