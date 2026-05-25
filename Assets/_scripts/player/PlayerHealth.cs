using UnityEngine;
using System;

[Serializable]
public class PlayerHealth //First darft of Health system
{
    public float MaxHealth = 100f;
    private float _currentHealth;
    public void Init()
    {
        _currentHealth = MaxHealth;
    }

    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player ist gestorben!");
        // Hier Gameover logik bzw event.
    }
}
