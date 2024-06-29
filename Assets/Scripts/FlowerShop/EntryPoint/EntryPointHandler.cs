using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FlowerShop.EntryPoint
{
    public class EntryPointHandler : MonoBehaviour
    {
        [SerializeField] private Scrollbar loadingScrollbar;

        private AsyncOperation loadingSceneOperation;

        private void Start()
        {
            loadingSceneOperation = SceneManager.LoadSceneAsync(1);
        }

        private void Update()
        {
            loadingScrollbar.size = loadingSceneOperation.progress;
        }
    }
}
