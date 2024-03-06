using System.Collections.Generic;
using UnityEngine;

namespace FlowerShop.Flowers
{
    public class FlowersCanvasLiaison : MonoBehaviour
    {
        [SerializeField] private List<FlowerInfoButton> flowerInfoButtons;
        
        [field: SerializeField] public Canvas FlowersCanvas { get; private set; }
        
        public void ShowCrossedFlowerInfo(FlowerInfo crossedFlowerInfo)
        {
            foreach (FlowerInfoButton flowerInfoButton in flowerInfoButtons)
            {
                flowerInfoButton.TrySetIsCrossedTrue(crossedFlowerInfo);
            }
        }
    }
}