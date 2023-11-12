using UnityEngine;

public class ModelViewAll : MonoBehaviour
{
    [Header("Model-View")]
    [SerializeField] private ImprovementModelView improvementModelView;
    [Header("Canvas")]
    [SerializeField] private Canvas improvementCanvas;

    public ImprovementModelView ImprovementModelView
    {
        get => improvementModelView;
    }

    public Canvas ImprovementCanvas
    {
        get => improvementCanvas;
    }
}
