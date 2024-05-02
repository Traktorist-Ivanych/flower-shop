using UnityEngine;
using UnityEngine.UI;

namespace FlowerShop.ComputerPages
{
    public class ComputerMainPageCanvasLiaison : MonoBehaviour
    {
        [SerializeField] private Image achievementsIndicator;
        
        [field: SerializeField] public Canvas ComputerMainPageCanvas { get; private set; }
        [field: SerializeField] public Button ComplaintsButton { get; private set; }
        [field: SerializeField] public Button VipButton { get; private set; }
        [field: SerializeField] public Button StatisticsButton { get; private set; }
        [field: SerializeField] public Button AchievementsButton { get; private set; }
        [field: SerializeField] public Button SettingsButton { get; private set; }

        public void ShowAchievementsIndicator()
        {
            achievementsIndicator.enabled = true;
        }

        public void HideAchievementsIndicator()
        {
            achievementsIndicator.enabled = false;
        }
    }
}