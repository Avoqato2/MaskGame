using UnityEngine;
using System;


// Enemys take damage an has invincible frames DamageCooldown = frames how long the enemy cant have damage
[Serializable]
public class EnemyHealth
{
    public float MaxHealth = 100f;
    public float ApplyDamageCooldown = 0.5f;
    
    [Header("Loot Settings")]
    public GameObject EssencePrefab; // Hier ziehst du dein neues Prefab rein!
    public int MinLoot = 3;
    public int MaxLoot = 6;
    
    private float _currentHealth;
    private float _nextPossibleHitTime;
    private GameObject _enemyObject;

    public void Init(GameObject enemyObject)
    {
        _enemyObject = enemyObject;
        _currentHealth = MaxHealth;
        _nextPossibleHitTime = 0f;
    }

    public void TakeDamage(float damageAmount) 
    {
        if (Time.time < _nextPossibleHitTime) 
        {
            return;
        }
        
        _currentHealth -= damageAmount;
        Debug.Log("Gegner hat Schaden bekommen! Rest: " + _currentHealth);
        _nextPossibleHitTime = Time.time + ApplyDamageCooldown;

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        int lootAmount = UnityEngine.Random.Range(MinLoot, MaxLoot + 1); 
        for (int i = 0; i < lootAmount; i++)
        {
            Vector3 spawnPosition = _enemyObject.transform.position + Vector3.up;
            GameObject.Instantiate(EssencePrefab, spawnPosition, Quaternion.identity);
        }
        GameObject.Destroy(_enemyObject);
    }
}