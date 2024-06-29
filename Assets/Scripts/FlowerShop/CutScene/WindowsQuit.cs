using UnityEngine;
using UnityEngine.SceneManagement;

public class WindowsQuit : MonoBehaviour
{
    public void QuitGame()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }

    public void ReloadLevel()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }
}
