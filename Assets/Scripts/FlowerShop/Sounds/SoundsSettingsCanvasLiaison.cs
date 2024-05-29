using UnityEngine;

namespace FlowerShop.Sounds
{
    public class SoundsSettingsCanvasLiaison : MonoBehaviour
    {
        [SerializeField] private Canvas soundsSettingsCanvas;

        public void ShowCanvas()
        {
            soundsSettingsCanvas.enabled = true;
        }

        public void HideCanvas()
        {
            soundsSettingsCanvas.enabled = false;
        }
    }
}
