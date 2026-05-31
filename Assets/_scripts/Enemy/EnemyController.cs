using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _turnSpeed;
    [SerializeField] Enemysensor _enemysensor;
    
    private Vector3 _targetDirection;
    private Quaternion _targetRotation;
    
    [Header("Enemy Health Settings")]
    [SerializeField] private EnemyHealth _enemyHealth;
    
    [Header("Aggro Settings")]
    [SerializeField] private float _aggroRange = 15f;
    [SerializeField] private float _aggroCheckInterval = 0.5f; 

    private bool _hasAggro; 
    private float _nextAggroCheckTime;
    
    private PlayerController _playerController;
    private Rigidbody _rigidbody;
    
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _enemyHealth.Init(gameObject);
        _targetRotation = _rigidbody.rotation;
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            _playerController = playerObj.GetComponent<PlayerController>();
        }
        
    }

    private void Update()
    {
        if (_playerController == null) return;
        
        if (Time.time >= _nextAggroCheckTime)
        {
            CheckAggroRange();
        }
        if (_hasAggro)
        {
            GetTargetDirection();
            if (_targetDirection != Vector3.zero) 
            {
                _targetRotation = Quaternion.LookRotation(_targetDirection);
            }
        }
        
    }
    
    private void CheckAggroRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _playerController.transform.position);
        _hasAggro = distanceToPlayer <= _aggroRange;
        _nextAggroCheckTime = Time.time + _aggroCheckInterval;
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
        if (_hasAggro && !_enemysensor.IsInrange)
        {
            Vector3 velocity = VelocityCalc(_moveSpeed);
            _rigidbody.linearVelocity = velocity;
        }
        else
        {
            _rigidbody.linearVelocity = new Vector3(0, _rigidbody.linearVelocity.y, 0); 
        }
    }
    
    private Vector3 VelocityCalc(float speed)
    {
        Vector3 velocity = _targetDirection * speed;
        velocity.y = _rigidbody.linearVelocity.y;
        return velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Damage"))
        {
            DamageDealer dealer = other.GetComponent<DamageDealer>();
        
            if (dealer != null)
            {
                _enemyHealth.TakeDamage(dealer.Damage); 
            }
        }
    }
    
}
