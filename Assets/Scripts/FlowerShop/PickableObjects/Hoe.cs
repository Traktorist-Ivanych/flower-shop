using System.Collections;
using FlowerShop.Flowers;
using PlayerControl;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(DinamicObjectMoving))]
public class Hoe : MonoBehaviour, IPickableObject
{
    [Inject] private readonly PlayerPickableObjectHandler playerPickableObjectHandler;
    [Inject] private readonly PlayerBusyness playerBusyness;
    [Inject] private readonly PlayerComponents playerComponents;
    [Inject] private readonly GameConfiguration gameConfiguration;

    [field: SerializeField] public GrowingRoom GrowingRoom { get; private set; }
    [SerializeField] private Mesh[] hoeLvlMeshes = new Mesh[2];

    private DinamicObjectMoving hoeMoving;
    private MeshFilter hoeMeshFilter;
    private int hoeLvl;

    private void Start()
    {
        hoeMoving = GetComponent<DinamicObjectMoving>();
        hoeMeshFilter = GetComponent<MeshFilter>();
    }

    public void TakeHoe()
    {
        playerPickableObjectHandler.CurrentPickableObject = this;
        hoeMoving.PutLittleDinamicObjectInPlayerHandsWithRotation();
    }

    public void GiveHoe(Transform targetTransfom)
    {
        hoeMoving.PutLittleDinamicObjectOnTableWithRotation(targetTransfom);
    }

    // with hue is redundant
    public IEnumerator DeleteWeedWithHoe(Pot potForDeletingWeed, WeedPlanter weedPlanterToAddPotInList)
    {
        playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.StartWeedingTrigger);
        yield return new WaitForSeconds(gameConfiguration.WeedingTime - gameConfiguration.WeedingTimeLvlDelta * hoeLvl);
        playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.FinishWeedingTrigger);
        potForDeletingWeed.DeleteWeed();
        weedPlanterToAddPotInList.AddPotInPlantingWeedList(potForDeletingWeed);
        playerBusyness.SetPlayerFree();
    }

    // hoe is redundant
    public void ImproveHoe()
    {
        hoeLvl++;
        hoeMeshFilter.mesh = hoeLvlMeshes[hoeLvl - 1];
    }
}
