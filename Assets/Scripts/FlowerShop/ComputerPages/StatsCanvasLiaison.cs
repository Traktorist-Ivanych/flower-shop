using System.ComponentModel;
using FlowerShop.Flowers;
using UnityEngine;
using UnityWeld.Binding;
using Zenject;

namespace FlowerShop.ComputerPages
{
    [Binding]
    public class StatsCanvasLiaison : MonoBehaviour, INotifyPropertyChanged
    {
        [Inject] private readonly FlowersSettings flowersSettings;
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        [Binding]
        public string GradesCount { get; private set; }
        [Binding]
        public string FiveStars { get; private set; }
        [Binding]
        public string FourStars { get; private set; }
        [Binding]
        public string TreeStars { get; private set; }
        [Binding]
        public string TwoStars { get; private set; }
        [Binding]
        public string OneStar { get; private set; }

        public void UpdateStatsCanvas(int gradesCount, int fiveStars, int fourStars, int treeStars,
            int twoStars, int oneStar)
        {
            GradesCount = gradesCount + "/" + flowersSettings.MaxGradesCount;
            OnPropertyChanged(nameof(GradesCount));
            
            FiveStars = fiveStars.ToString();
            OnPropertyChanged(nameof(FiveStars));

            FourStars = fourStars.ToString();
            OnPropertyChanged(nameof(FourStars));

            TreeStars = treeStars.ToString();
            OnPropertyChanged(nameof(TreeStars));

            TwoStars = twoStars.ToString();
            OnPropertyChanged(nameof(TwoStars));

            OneStar = oneStar.ToString();
            OnPropertyChanged(nameof(OneStar));
        }
        
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}