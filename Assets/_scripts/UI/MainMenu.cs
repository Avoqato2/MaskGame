using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public void StartClicked()
    {
        SceneManager.LoadScene("Hub");
    }
    
    public void QuitClicked()
    {
        Debug.Log("Quit button clicked");
        Application.Quit();
    }
}
