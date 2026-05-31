using UnityEngine;

public class Enemysensor : MonoBehaviour
{
    public bool IsInrange { get; private set; }
    
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        { 
            IsInrange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsInrange = false;
        }
    }
    
}

