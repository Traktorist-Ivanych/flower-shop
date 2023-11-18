using System.Collections;
using UnityEngine;
using Zenject;

public class BreakableFlowerTableBase : MonoBehaviour, IBreakableTable
{
    [Inject] private readonly GameConfiguration gameConfiguration;
    [Inject] private readonly PlayerComponents playerComponents;
    [Inject] private readonly PlayerBusyness playerBusyness;

    [SerializeField] private MeshRenderer breakdownIndicatorRenderer;
    [SerializeField] private ParticleSystem[] brokenParticleSystems;

    private bool isTableBroken;
    private int actionsBeforeBrokenQuantity;

    public bool IsTableBroken
    {
        get => isTableBroken;
    } 

    public void HideBreakdownIndicator()
    {
        breakdownIndicatorRenderer.enabled = false;
    }

    public void ShowBreakdownIndicator()
    {
        breakdownIndicatorRenderer.enabled = true;
    }

    public void UseBreakableFlowerTable()
    {
        actionsBeforeBrokenQuantity--;

        if (actionsBeforeBrokenQuantity <= 0)
        {
            isTableBroken = true;

            foreach (ParticleSystem brokenPS in brokenParticleSystems)
            {
                brokenPS.Play();
            }
        }
    }

    public void FixBreakableFlowerTable(int minQuantity, int maxQuantity)
    {
        playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.StartBuildsTrigger);
        breakdownIndicatorRenderer.enabled = false;
        SetActionsBeforeBrokenQuantity(minQuantity, maxQuantity);

        StartCoroutine(FixBreakableFlowerTableProcess());
    }

    public void SetActionsBeforeBrokenQuantity(int minQuantity, int maxQuantity)
    {
        actionsBeforeBrokenQuantity = Random.Range(minQuantity, maxQuantity);
    }

    private IEnumerator FixBreakableFlowerTableProcess()
    {
        yield return new WaitForSeconds(gameConfiguration.TableRepairTime);

        playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.FinishBuildsTrigger);
        isTableBroken = false;
        playerBusyness.SetPlayerFree();

        foreach (ParticleSystem brokenPS in brokenParticleSystems)
        {
            brokenPS.Stop();
        }
    }
}
