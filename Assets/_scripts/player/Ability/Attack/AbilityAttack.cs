using UnityEngine;
using System;

[Serializable]
public class AbilityAttack 
{
    
    
    public float projectileSpeed = 10f;
    public float projectileDamage = 10f;
    public float projectileDiameter = 5f;
    public float AttackTime = 3f;
    public float AttackCooldown = 1.5f;
    public float ProjectileOffset = 2f;
    
    public bool IsAttacking { get; private set; }
    private float _attackEndTime;
    private float _nextAttackTime;
    
    private PlayerController _playerController;
    private float projectileRadius => projectileDiameter / 2f;

    public void Init(PlayerController playerController)
    {
        _playerController = playerController;
    }

    public void TryAttack()
    {
        if (!IsAttacking && Time.time >= _nextAttackTime)
        {
            IsAttacking = true;
            _attackEndTime = Time.time + AttackTime;
           Transform playerTransform = _playerController.transform;
           Vector3 currentInput = _playerController.CurrentMovementInput;
           
           SpawnProjectile(playerTransform, currentInput);
        }
    }

    public void UpdateAttack(Transform playerTransform, Vector3 movementInput)
    {
        if(!IsAttacking)return;
        if (Time.time >= _attackEndTime)
        {
            IsAttacking = false;
            _nextAttackTime = Time.time + AttackCooldown;
            return;
        }
    }
    
    private void SpawnProjectile(Transform transform, Vector3 currentInput)
    {
        Vector3 shootDirection = currentInput;
        if (shootDirection == Vector3.zero)
        {
            shootDirection = transform.forward;
        }
        
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = Vector3.one * projectileDiameter;
        sphere.transform.position = transform.position + (shootDirection * ProjectileOffset);
        sphere.transform.position = new Vector3(sphere.transform.position.x ,projectileRadius, sphere.transform.position.z);
        
        Collider collider = sphere.GetComponent<Collider>();
        collider.isTrigger = true;
        
        ProjectileMovement projectileMovement = sphere.AddComponent<ProjectileMovement>();
        projectileMovement.Init(shootDirection, projectileSpeed, projectileDamage, AttackTime);
    }
}
