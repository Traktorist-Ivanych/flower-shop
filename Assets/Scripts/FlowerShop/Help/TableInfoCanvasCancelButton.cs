using FlowerShop.ComputerPages;
using FlowerShop.Effects;
using PlayerControl;
using UnityEngine;
using Zenject;

namespace FlowerShop.Help
{
    [RequireComponent(typeof(UIButton))]
    public class TableInfoCanvasCancelButton : MonoBehaviour
    {
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
            tableInfoCanvasLiason.HideCanvas();
            playerBusyness.SetPlayerFree();
            selectedTableEffect.ActivateEffectWithDelay();
        }
    }
}
