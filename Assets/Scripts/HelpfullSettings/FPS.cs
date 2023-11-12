using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText;

    private void Start()
    {
        StartCoroutine(ShowFPS());
    }

    private IEnumerator ShowFPS()
    {
        yield return new WaitForSeconds(0.5f);

        fpsText.text = Mathf.Round(1 / Time.deltaTime).ToString();
        StartCoroutine(ShowFPS());
    }
}
