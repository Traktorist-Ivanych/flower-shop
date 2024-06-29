using FlowerShop.ComputerPages;
using FlowerShop.Education;
using FlowerShop.Effects;
using Input;
using PlayerControl;
using UnityEngine;
using Zenject;

namespace FlowerShop.Help
{
    [RequireComponent(typeof(UIButton))]
    public class TableInfoCanvasCancelButton : MonoBehaviour
    {
        [Inject] private readonly EducationHandler educationHandler;
        [Inject] private readonly PlayerBusyness playerBusyness;
        [Inject] private readonly SelectedTableEffect selectedTableEffect;
        [Inject] private readonly TableInfoCanvasLiaison tableInfoCanvasLiason;

        [HideInInspector, SerializeField] private UIButton button;

        private void OnValidate()
        {
            button = GetComponent<UIButton>();
        }

        private void OnEnable()
        {
            button.OnClickEvent += OnButtonClick;
        }

        private void OnDisable()
        {
            button.OnClickEvent -= OnButtonClick;
        }

        private void OnButtonClick()
        {
            if (educationHandler.IsEducationActive)
            {
                if (educationHandler.IsMonoBehaviourCurrentEducationStep(this))
                {
                    tableInfoCanvasLiason.HideCanvas();
                    playerBusyness.SetPlayerFree();
                    selectedTableEffect.ActivateEffectWithDelay();
                    educationHandler.CompleteEducationStep();
                }
            }
            else
            {
                tableInfoCanvasLiason.HideCanvas();
                playerBusyness.SetPlayerFree();
                selectedTableEffect.ActivateEffectWithDelay();
            }
        }
    }
}
