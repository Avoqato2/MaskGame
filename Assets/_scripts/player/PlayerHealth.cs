using UnityEngine;
using System;

[Serializable]
public class PlayerHealth //First draft of Health system
{
    public static event Action PlayerDead;
    public float MaxHealth = 100f;
    private float _currentHealth;
    public void Init(PlayerUI playerUI)
    {
        _currentHealth = MaxHealth;
        if (playerUI != null)
        {
            playerUI.UpdateHealth(_currentHealth, MaxHealth);
        }
    }

    public void TakeDamage(float amount, PlayerUI playerUI)
    {
        _currentHealth -= amount;
        if (playerUI != null)
        {
            playerUI.UpdateHealth(_currentHealth, MaxHealth);
        }
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player ist gestorben!");
        PlayerDead?.Invoke();
    }
}
