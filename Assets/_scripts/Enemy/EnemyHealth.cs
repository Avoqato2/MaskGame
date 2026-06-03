using UnityEngine;
using System;


// Enemys take damage an has invincible frames DamageCooldown = frames how long the enemy cant have damage
[Serializable]
public class EnemyHealth
{
    public float MaxHealth = 100f;
    public float ApplyDamageCooldown = 0.5f;
    
    [Header("Loot Settings")]
    public int MinLoot = 3;
    public int MaxLoot = 6;
    
    private ObjektPool<LootEssence> _sharedLootPool;
    private float _currentHealth;
    private float _nextPossibleHitTime;
    private GameObject _enemyObject;

    public void Init(GameObject enemyObject, ObjektPool<LootEssence> sharedLootPool)
    {
        _enemyObject = enemyObject;
        _currentHealth = MaxHealth;
        _nextPossibleHitTime = 0f;

        _sharedLootPool = sharedLootPool;
    }
    
    public void ResetHealth() 
    {
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

            LootEssence loot = _sharedLootPool.GetPoolObjekt(spawnPosition, Quaternion.identity);
            loot.transform.position = spawnPosition;
        }
        GameObject.Destroy(_enemyObject);
    }
}