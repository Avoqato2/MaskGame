using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _turnSpeed;
    [SerializeField] PlayerController _playerController;
    [SerializeField] Enemysensor _enemysensor;
    
    [Header("Enemy Health Settings")]
    [SerializeField] private EnemyHealth _enemyHealth;
    
    private Rigidbody _rigidbody;
    private Vector3 _targetDirection;
    private Quaternion _targetRotation;
    
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _enemyHealth.Init(GameObject.Find("Enemy"));
        _targetRotation = _rigidbody.rotation;
        
    }

    private void Update()
    {
        GetTargetDirection();
        if (_targetDirection != Vector3.zero) // "null" exeption
        {
            _targetRotation = Quaternion.LookRotation(_targetDirection);
        }
        
    }

    private void GetTargetDirection()
    {
         Vector3 _playerPosition = _playerController.transform.position;
         Vector3 _currentPosition = transform.position;
        _targetDirection = (_playerPosition - _currentPosition).normalized;
        _targetDirection = new Vector3(_targetDirection.x, 0, _targetDirection.z);
        
    }

    private void FixedUpdate()
    {
        _rigidbody.MoveRotation(Quaternion.Slerp(_rigidbody.rotation, _targetRotation,
                _turnSpeed * Time.fixedDeltaTime));
        if (!_enemysensor.IsInrange)
        {
            Vector3 velocity = VelocityCalc(_moveSpeed);
            _rigidbody.linearVelocity = velocity;

        }
        else
        {
            _rigidbody.linearVelocity = Vector3.zero;
        }
    }
    
    private Vector3 VelocityCalc(float speed)
    {
        Vector3 velocity = _targetDirection * speed;
        return velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerArm")
        {
            _enemyHealth.TakeDamage();
        } 
    }
    
}
