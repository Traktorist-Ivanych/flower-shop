using System.Collections;
using TMPro;
using UnityEngine;

#if UNITY_EDITOR || DEVELOPMENT_BUILD 
namespace DebugSetting
{
    public class FpsDisplayer : MonoBehaviour
    {
        [SerializeField] private Canvas fpsCanvas;
        [SerializeField] private TextMeshProUGUI fpsText;

        private void Start()
        {
            fpsCanvas.enabled = true;
            StartCoroutine(ShowFPS());
        }

        private IEnumerator ShowFPS()
        {
            yield return new WaitForSeconds(0.5f);

            fpsText.text = Mathf.Round(1 / Time.deltaTime).ToString();
            StartCoroutine(ShowFPS());
        }
    }
}
#endif
