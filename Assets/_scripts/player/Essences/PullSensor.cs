using System;
using UnityEngine;

public class PullSensor : MonoBehaviour
{
    public event Action<LootEssence> LootEntered;
    public event Action<LootEssence> LootExited;
    private void OnTriggerEnter(Collider other)
    {
        LootEssence pickup = other.GetComponent<LootEssence>();
        if (null != pickup)
        {
            LootEntered?.Invoke(pickup);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        LootEssence pickup = other.GetComponent<LootEssence>();
        if (null != pickup)
        {
            LootExited?.Invoke(pickup);
        }
    }
}