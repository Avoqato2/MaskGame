using UnityEngine;
using System;
[Serializable]
public class EnemyDamage
{
    public float Damage = 10f;
    public float DamageCooldown = 1.5f; // cooldown on when the enemy hits again
    
    private float _nextDamageTime;
    
    public bool CanDealDamage()
    {
        if (Time.time >= _nextDamageTime)
        {
            _nextDamageTime = Time.time + DamageCooldown;
            return true;
        }
        return false;
    }
    
    
}
