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
    
    public bool IsAttacking { get; private set; }
    private float _attackEndTime;
    private float _nextAttackTime;
    
    private float projectileRadius => projectileDiameter / 2f;
    private float _offset = 2f;
    
    

    public void TryAttack()
    {
        if (!IsAttacking && Time.time >= _nextAttackTime)
        {
            IsAttacking = true;
            _attackEndTime = Time.time + AttackTime;
            Debug.Log("atttaaack");
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
    
    private GameObject Projectile(Transform transform)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = Vector3.one * projectileDiameter;
        Collider collider = sphere.GetComponent<Collider>();
        collider.isTrigger = true;
        sphere.transform.position = new Vector3(transform.position.x ,projectileRadius, transform.position.z + _offset + projectileRadius);
        return sphere;
    }
}
