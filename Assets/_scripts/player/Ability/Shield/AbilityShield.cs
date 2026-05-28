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
    
    private PlayerController _playerController;
    
    public void Init(PlayerController playerController)
    {
        _playerController = playerController;
    }
    
    public void TryShield()
    {
        if (!IsInvincible && Time.time >= _nextShieldTime)
        {
            IsInvincible = true;
            _shieldEndTime = Time.time + ShieldTime;
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
            Debug.Log("Shield deactivated!");
        }
        // das gehört irgwie ausgelagert weil damage health stuff sollte hier ja nicht rein
    }
}
