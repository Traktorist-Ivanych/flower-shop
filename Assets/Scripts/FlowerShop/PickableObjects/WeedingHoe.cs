using System.Collections;
using FlowerShop.Flowers;
using PlayerControl;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(OldPickableObjectMoving))]
public class WeedingHoe : MonoBehaviour, IPickableObject
{
    [Inject] private readonly PlayerPickableObjectHandler playerPickableObjectHandler;
    [Inject] private readonly PlayerBusyness playerBusyness;
    [Inject] private readonly PlayerComponents playerComponents;
    [Inject] private readonly GameConfiguration gameConfiguration;

    [field: SerializeField] public GrowingRoom GrowingRoom { get; private set; }
    [SerializeField] private Mesh[] hoeLvlMeshes = new Mesh[2];

    private OldPickableObjectMoving hoeMoving;
    private MeshFilter hoeMeshFilter;
    private int hoeLvl;

    private void Start()
    {
        hoeMoving = GetComponent<OldPickableObjectMoving>();
        hoeMeshFilter = GetComponent<MeshFilter>();
    }

    public void TakeInPlayerHands()
    {
        playerPickableObjectHandler.CurrentPickableObject = this;
        hoeMoving.TakeLittlePickableObjectInPlayerHands();
    }

    public void PutOnTable(Transform targetTransform)
    {
        hoeMoving.PutLittlePickableObjectOnTable(targetTransform);
    }

    public IEnumerator DeleteWeed(Pot potForDeletingWeed, WeedPlanter weedPlanterToAddPotIntoList)
    {
        playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.StartWeedingTrigger);
        yield return new WaitForSeconds(gameConfiguration.WeedingTime - gameConfiguration.WeedingTimeLvlDelta * hoeLvl);
        playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.FinishWeedingTrigger);
        potForDeletingWeed.DeleteWeed();
        weedPlanterToAddPotIntoList.AddPotInPlantingWeedList(potForDeletingWeed);
        playerBusyness.SetPlayerFree();
    }

    // hoe is redundant
    public void ImproveHoe()
    {
        hoeLvl++;
        hoeMeshFilter.mesh = hoeLvlMeshes[hoeLvl - 1];
    }
}
