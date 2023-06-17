using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{

    public static void SwitchToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
