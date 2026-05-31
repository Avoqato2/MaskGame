using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    private void OnEnable()
    {
        LoadingZone.OnLoadingZoneEntered += HandleLevelTransition;
    }

    private void OnDisable()
    {
        LoadingZone.OnLoadingZoneEntered -= HandleLevelTransition;
    }

    private void HandleLevelTransition(string sceneName)
    {
        SceneManager.LoadScene(sceneName); // Load Scene
    }
}
