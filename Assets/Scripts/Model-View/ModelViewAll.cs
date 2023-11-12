using UnityEngine;

public class ModelViewAll : MonoBehaviour
{
    [Header("Model-View")]
    [SerializeField] private ImprovementModelView improvementModelView;
    [SerializeField] private PlayerStatsModelView playerStatsModelView;
    [Header("Canvas")]
    [SerializeField] private Canvas improvementCanvas;
    [SerializeField] private Canvas playerStatsCanvas;

    public ImprovementModelView ImprovementModelView
    {
        get => improvementModelView;
    }

    public PlayerStatsModelView PlayerStatsModelView
    {
        get => playerStatsModelView;
    }

    public Canvas ImprovementCanvas
    {
        get => improvementCanvas;
    }

    public Canvas PlayerStatsCanvas
    {
        get => playerStatsCanvas;
    }
}
