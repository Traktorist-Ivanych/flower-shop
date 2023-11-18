using UnityEngine;

public class AllCanvasLiaisons : MonoBehaviour
{
    [Header("Model-View")]
    [SerializeField] private ImprovementCanvasLiaison improvementCanvasLiaison;
    [SerializeField] private PlayerStatsCanvasLiaison playerStatsCanvasLiaison;
    [SerializeField] private CoffeCanvasLiaison coffeCanvasLiaison;
    [Header("Canvas")]
    [SerializeField] private Canvas improvementCanvas;
    [SerializeField] private Canvas playerStatsCanvas;
    [SerializeField] private Canvas coffeCanvas;

    public ImprovementCanvasLiaison ImprovementCanvasLiaison
    {
        get => improvementCanvasLiaison;
    }

    public PlayerStatsCanvasLiaison PlayerStatsCanvasLiaison
    {
        get => playerStatsCanvasLiaison;
    }

    public CoffeCanvasLiaison CoffeCanvasLiaison
    {
        get => coffeCanvasLiaison;
    }

    public Canvas ImprovementCanvas
    {
        get => improvementCanvas;
    }

    public Canvas PlayerStatsCanvas
    {
        get => playerStatsCanvas;
    }

    public Canvas CoffeCanvas
    {
        get => coffeCanvas;
    }
}
