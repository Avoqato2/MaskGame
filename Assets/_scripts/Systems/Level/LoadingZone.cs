using UnityEngine;
using System;

public class LoadingZone : MonoBehaviour
{ 
    [Header("Settings")]
    [SerializeField] private string _sceneToLoad; 
    
    public static event Action<string> OnLoadingZoneEntered; 

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player")) 
        {
            OnLoadingZoneEntered?.Invoke(_sceneToLoad); // Fire event for LevelManager
        }
    }
}
