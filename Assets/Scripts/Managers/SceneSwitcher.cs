using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{

    public static void SwitchToScene(string sceneName)
    {
        UIManager.Instance.CloseAllScreens();
        SceneManager.LoadScene(sceneName);
    }
}
