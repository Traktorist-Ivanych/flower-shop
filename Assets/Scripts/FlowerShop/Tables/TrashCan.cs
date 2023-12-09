using FlowerShop.Flowers;
using FlowerShop.PickableObjects;
using PlayerControl;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class TrashCan : FlowerTable
{
    [Inject] private readonly PlayerComponents playerComponents;

    [SerializeField] private Animator trashCanAnimator;
    [SerializeField] private MeshRenderer flowerRenderer;
    [SerializeField] private MeshRenderer soilRenderer;
    [SerializeField] private MeshRenderer weedRenderer;
    [FormerlySerializedAs("flowerEmpty")] [SerializeField] private FlowerName flowerNameEmpty;

    private Pot playerPot;
    private MeshFilter flowerMeshFilter;
    private MeshFilter weedMeshFilter;
    private static readonly int Throw = Animator.StringToHash("Throw");

    private void Start()
    {
        flowerMeshFilter = flowerRenderer.GetComponent<MeshFilter>();
        weedMeshFilter = weedRenderer.GetComponent<MeshFilter>();
    }

    public override void ExecuteClickableAbility()
    {
        if (playerBusyness.IsPlayerFree && playerPickableObjectHandler.CurrentPickableObject is Pot)
        {
            playerPot = playerPickableObjectHandler.CurrentPickableObject as Pot;

            if (playerPot.IsSoilInsidePot)
            {
                SetPlayerDestination();
            }
        }
    }

    public override void ExecutePlayerAbility()
    {
        playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.ThrowTrigger);
        trashCanAnimator.SetTrigger(Throw);

        if (playerPot.PlantedFlowerInfo.FlowerName != flowerNameEmpty)
        {
            flowerRenderer.enabled = true;
            flowerMeshFilter.mesh = playerPot.PotObjects.FlowerMeshFilter.mesh;
        }
        if (playerPot.IsWeedInPot)
        {
            weedRenderer.enabled = true;
            weedMeshFilter.mesh = playerPot.PotObjects.WeedMeshFilter.mesh;
        }
        soilRenderer.enabled = true;

        playerPot.CleanPot();
    }

    public void FinishThrowingTrash()
    {
        flowerRenderer.enabled = false;
        soilRenderer.enabled = false;
        weedRenderer.enabled = false;
        playerBusyness.SetPlayerFree();
    }
}
