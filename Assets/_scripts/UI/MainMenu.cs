using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioSource clickSound;
    public void StartClicked()
    {
        StartCoroutine(LoadSceneAfterSound("IntroStory"));
    }
    
    public void QuitClicked()
    {
        Debug.Log("Quit button clicked");
        Application.Quit();
    }
    
    private IEnumerator LoadSceneAfterSound(string sceneName)
    {
        if (clickSound != null)
        {
            clickSound.Play();
            yield return new WaitForSeconds(clickSound.clip.length);
        }
        
        SceneManager.LoadScene(sceneName);
    }
}
