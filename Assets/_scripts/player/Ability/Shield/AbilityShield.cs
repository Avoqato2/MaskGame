using UnityEngine;
using System;
[Serializable]
public class AbilityShield
{
    public float ShieldTime = 2f;
    public float ShieldCooldown = 3f;
    
    public bool IsInvincible { get; private set; }
    private float _shieldEndTime;
    private float _nextShieldTime;
    
    private GameObject _shield;
    
    public void Init(GameObject playerObject)
    {
        _shield = playerObject.transform.Find("Shield").gameObject;
    }
    
    public void TryShield()
    {
        if (!IsInvincible && Time.time >= _nextShieldTime)
        {
            IsInvincible = true;
            _shieldEndTime = Time.time + ShieldTime;
            _shield.SetActive(IsInvincible);
            Debug.Log("nooooo damage for me");
        }
    }
    public void UpdateShield()
    {
        if(!IsInvincible) return;
        if (Time.time >= _shieldEndTime)
        {
            IsInvincible = false;
            _nextShieldTime = Time.time + ShieldCooldown;
            _shield.SetActive(IsInvincible);
            Debug.Log("Shield deactivated!");
        }
    }
}
