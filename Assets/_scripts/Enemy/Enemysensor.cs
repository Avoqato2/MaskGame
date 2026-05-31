using UnityEngine;


//A script for telling the enemy when they should stop.
// The collider in the enemy is also named Enemysensor
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

