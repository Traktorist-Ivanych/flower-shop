using System.Collections;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(DinamicObjectMoving))]
public class Hoe : MonoBehaviour, IDinamicObject, IGrowingRoom
{
    [Inject] private readonly CurrentPlayerDinamicObject playerDinamicObject;
    [Inject] private readonly PlayerBusyness playerBusyness;
    [Inject] private readonly PlayerComponents playerComponents;
    [Inject] private readonly GameConfiguration gameConfiguration;

    [SerializeField] private IGrowingRoom.GroweringRoom groweringRoom;
    [SerializeField] private Mesh[] hoeLvlMeshes = new Mesh[2];

    private DinamicObjectMoving hoeMoving;
    private MeshFilter hoeMeshFilter;
    private int hoeLvl;

    public IGrowingRoom.GroweringRoom GetGroweringRoom() { return groweringRoom; }

    private void Start()
    {
        hoeMoving = GetComponent<DinamicObjectMoving>();
        hoeMeshFilter = GetComponent<MeshFilter>();
    }

    public void TakeHoe()
    {
        playerDinamicObject.SetPlayerDinamicObject(this);
        hoeMoving.PutLittleDinamicObjectInPlayerHandsWithRotation();
    }

    public void GiveHoe(Transform targetTransfom)
    {
        hoeMoving.PutLittleDinamicObjectOnTableWithRotation(targetTransfom);
    }

    public IEnumerator DeleteWeedWithHoe(Pot potForDeletingWeed, WeedPlanter weedPlanterToAddPotInList)
    {
        playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.StartWeedingTrigger);
        yield return new WaitForSeconds(gameConfiguration.WeedingTime - gameConfiguration.WeedingTimeLvlDelta * hoeLvl);
        playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.FinishWeedingTrigger);
        potForDeletingWeed.DeleteWeed();
        weedPlanterToAddPotInList.AddPotInPlantingWeedList(potForDeletingWeed);
        playerBusyness.SetPlayerFree();
    }

    public void ImproveHoe()
    {
        hoeLvl++;
        hoeMeshFilter.mesh = hoeLvlMeshes[hoeLvl - 1];
    }
}
