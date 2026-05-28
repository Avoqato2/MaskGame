using UnityEngine;
using System;

public class LoadingZone : MonoBehaviour
{ [Header("Settings")]
    [SerializeField] private string _sceneToLoad; 
    
    public static event Action<string> OnLoadingZoneEntered; 

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player")) 
        {
            Debug.Log($"Spieler hat Loading Zone betreten. Lade Szene: {_sceneToLoad}");
            
            OnLoadingZoneEntered?.Invoke(_sceneToLoad);
        }
    }
}
