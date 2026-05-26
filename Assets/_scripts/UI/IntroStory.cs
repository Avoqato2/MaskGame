using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroStory : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.LoadScene("Hub", LoadSceneMode.Single);
    }
    
}
