﻿using System.ComponentModel;
using Input;
using UnityEngine;
using UnityWeld.Binding;
using Zenject;

namespace FlowerShop.Fertilizers
{
    [Binding]
    public class FertilizersCanvasLiaison : MonoBehaviour, INotifyPropertyChanged
    {
        [Inject] private readonly FertilizersSetting fertilizersSetting;
        [Inject] private readonly PlayerInputActions playerInputActions;
        
        [SerializeField] private GrowthAccelerator growthAccelerator;
        [SerializeField] private GrowingLvlIncreaser growingLvlIncreaser;
        [SerializeField] private GrowerToMaxLvl growerToMaxLvl;
        [SerializeField] private GameObject fertilizerInfoPanel;
        [SerializeField] private Canvas fertilizersCanvas;
        
        public event PropertyChangedEventHandler PropertyChanged;

        [Binding]
        public int GrowthAcceleratorAvailableUsesNumber => growthAccelerator.AvailableUsesNumber;
        
        [Binding]
        public int GrowingLvlIncreaserAvailableUsesNumber => growingLvlIncreaser.AvailableUsesNumber;
        
        [Binding]
        public int GrowerToMaxLvlAvailableUsesNumber => growerToMaxLvl.AvailableUsesNumber;
        
        [Binding]
        public string FertilizerName { get; private set; }
        
        [Binding]
        public Sprite FertilizerSprite { get; private set; }
        
        [Binding]
        public string FertilizerDescription { get; private set; }

        public void EnableCanvas()
        {
            playerInputActions.EnableCanvasControlMode();
            fertilizersCanvas.enabled = true;
        }
        
        public void DisableCanvas()
        {
            playerInputActions.DisableCanvasControlMode();
            fertilizersCanvas.enabled = false;
        }

        public void UpdateFertilizersAvailableUsesNumber()
        {
            OnPropertyChanged(nameof(GrowthAcceleratorAvailableUsesNumber));
            OnPropertyChanged(nameof(GrowingLvlIncreaserAvailableUsesNumber));
            OnPropertyChanged(nameof(GrowerToMaxLvlAvailableUsesNumber));
        }

        public void ShowFertilizerInfoPanel(FertilizerInfo fertilizerInfo)
        {
            fertilizerInfoPanel.SetActive(true);

            FertilizerName = fertilizerInfo.FertilizerName;
            OnPropertyChanged(nameof(FertilizerName));
            
            FertilizerSprite = fertilizerInfo.FertilizerSprite;
            OnPropertyChanged(nameof(FertilizerSprite));
            
            FertilizerDescription = fertilizerInfo.FertilizerDescription;
            OnPropertyChanged(nameof(FertilizerDescription));
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}