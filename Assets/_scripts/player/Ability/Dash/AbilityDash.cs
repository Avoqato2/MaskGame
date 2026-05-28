using UnityEngine;
using System;
[Serializable]
public class AbilityDash 
{
    public float DashSpeed = 20f;
    public float DashTime = 0.25f;
    public float DashCooldown = 1.5f;
    
    public bool IsDashing { get; private set; }
    private float _dashEndTime;
    private float _nextDashTime;
    
    private PlayerController _playerController;
    
    public void Init(PlayerController playerController)
    {
        _playerController = playerController;
    }
    
    public void TryDash()
    {
        if (!IsDashing && Time.time >= _nextDashTime)
        {
            IsDashing = true;
            _dashEndTime = Time.time + DashTime;
        }
    }
    
    public void UpdateDash(Rigidbody rigidbody, Vector3 movementInput,
        Transform playerTransform)
    {
        if(!IsDashing)return;
        if (Time.time >= _dashEndTime)
        {
            IsDashing = false;
            _nextDashTime = Time.time + DashCooldown;
            return;
        }
        
        Vector3 dashDirection = movementInput;
        if (dashDirection == Vector3.zero)
        {
            dashDirection = playerTransform.forward;
        }
        Vector3 dasVelocity = dashDirection * DashSpeed;
        dasVelocity.y = rigidbody.linearVelocity.y;
        rigidbody.linearVelocity = dasVelocity;
    }
}
