using System.Collections;
using UnityEngine;
using Zenject;

// rename to UpgradingHammer(andRepairing) (class should say what he does)
public class Hammer : MonoBehaviour, IDynamicObject
{
    [Inject] private readonly CurrentPlayerDinamicObject playerDinamicObject;
    [Inject] private readonly PlayerBusyness playerBusyness;
    [Inject] private readonly PlayerComponents playerComponents;
    [Inject] private readonly GameConfiguration gameConfiguration;

    private DinamicObjectMoving hammerMoving;
    private IImprovableTable improvableTable;

    public IImprovableTable ImprovableTable
    {
        set => improvableTable = value;
    }

    private void Start()
    {
        hammerMoving = GetComponent<DinamicObjectMoving>();
    }

    public void TakeHammer()
    {
        playerDinamicObject.SetPlayerDinamicObject(this);
        hammerMoving.PutLittleDinamicObjectInPlayerHandsWithRotation();
    }

    public void GiveHammer(Transform targetTransfom)
    {
        hammerMoving.PutLittleDinamicObjectOnTableWithRotation(targetTransfom);
    }

    public IEnumerator ImproveTable()
    {
        playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.StartBuildsTrigger);
        improvableTable.HideImprovableIndicator();
        yield return new WaitForSeconds(gameConfiguration.TableImprovementTime);
        playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.FinishBuildsTrigger);
        improvableTable.ImproveTable();
        playerBusyness.SetPlayerFree();
    }
}
