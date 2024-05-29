﻿using FlowerShop.Effects;
using Input;
using PlayerControl;
using UnityEngine;
using Zenject;

namespace FlowerShop.ComputerPages
{
    [RequireComponent(typeof(UIButton))]
    public class ComputerMainPageCancel : MonoBehaviour
    {
        [Inject] private readonly ComputerMainPageCanvasLiaison computerMainPageCanvasLiaison;
        [Inject] private readonly PlayerComponents playerComponents;
        [Inject] private readonly PlayerInputActions playerInputActions;
        [Inject] private readonly SelectedTableEffect selectedTableEffect;
        
        [HideInInspector, SerializeField] private UIButton uiButton;

        private void OnValidate()
        {
            uiButton = GetComponent<UIButton>();
        }

        private void OnEnable()
        {
            uiButton.OnClickEvent += OnButtonClick;
        }
        
        private void OnDisable()
        {
            uiButton.OnClickEvent -= OnButtonClick;
        }

        private void OnButtonClick()
        {
            selectedTableEffect.ActivateEffectWithDelay();
            computerMainPageCanvasLiaison.ComputerMainPageCanvas.enabled = false;
            playerInputActions.DisableCanvasControlMode();
            playerComponents.PlayerAnimator.SetTrigger(PlayerAnimatorParameters.StopUsingComputer);
        }
    }
}