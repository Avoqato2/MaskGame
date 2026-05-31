using UnityEngine;
using System;

[Serializable]
public class EnemyHealth
{
    public float MaxHealth = 100f;
    
    [Header("Hit Settings")]
    public float DamageCooldown = 0.5f;
    public float DamageAmount = 10f;
    
    private float _currentHealth;
    private float _nextPossibleHitTime;
    private GameObject _enemyObject;

    public void Init(GameObject enemyObject)
    {
        _enemyObject = enemyObject;
        _currentHealth = MaxHealth;
        _nextPossibleHitTime = 0f;
    }

    public void TakeDamage() 
    {
        if (Time.time < _nextPossibleHitTime) 
        {
            return;
        }
        
        _currentHealth -= DamageAmount;
        Debug.Log("Gegner hat Schaden bekommen! Rest: " + _currentHealth);
        _nextPossibleHitTime = Time.time + DamageCooldown;

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy ist tot!");
        GameObject.Destroy(_enemyObject);
    }
}