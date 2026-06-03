using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEditor;

[Serializable]
public class AbilityAttack 
{
    
    [SerializeField]public GameObject projectile;
    public float projectileSpeed = 10f;
    public float AttackTime = 3f;
    public float AttackCooldown = 1.5f;
    public float ProjectileOffset = 2f;
    
    public bool IsAttacking { get; private set; }
    private float _attackEndTime;
    private float _nextAttackTime;
    
    private PlayerController _playerController;

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
        
        Quaternion projectileRotation = Quaternion.LookRotation(shootDirection);
        GameObject SpawnedProjectile = GameObject.Instantiate(projectile, transform.position + shootDirection.normalized * ProjectileOffset, projectileRotation);
        SpawnedProjectile.transform.position = transform.position + (shootDirection * ProjectileOffset);
        SpawnedProjectile.transform.position = new Vector3(SpawnedProjectile.transform.position.x ,SpawnedProjectile.transform.localScale.y /2, SpawnedProjectile.transform.position.z);;
        Projectile projectileMovement = SpawnedProjectile.GetComponent<Projectile>();
        projectileMovement.Init(shootDirection, projectileSpeed, AttackTime);
    }
}
