using UnityEngine;
using UnityEngine.UI;

namespace FlowerShop.ComputerPages
{
    public class ComputerMainPageCanvasLiaison : MonoBehaviour
    {
        [SerializeField] private Image achievementsIndicator;

        private int achievementsIndicatorCount;

        [field: SerializeField] public Canvas ComputerMainPageCanvas { get; private set; }
        [field: SerializeField] public Button ComplaintsButton { get; private set; }
        [field: SerializeField] public Button VipButton { get; private set; }
        [field: SerializeField] public Button StatisticsButton { get; private set; }
        [field: SerializeField] public Button AchievementsButton { get; private set; }

        public void ShowAchievementsIndicator()
        {
            if (achievementsIndicatorCount == 0)
            {
                achievementsIndicator.enabled = true;
            }

            achievementsIndicatorCount++;
        }

        public void HideAchievementsIndicator()
        {
            achievementsIndicatorCount--;

            if (achievementsIndicatorCount == 0)
            {
                achievementsIndicator.enabled = false;
            }
        }
    }
}