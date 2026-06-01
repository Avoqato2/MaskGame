using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _turnSpeed;
    
    private Vector3 _targetDirection;
    private Quaternion _targetRotation;
    
    // thies setting are for moving the enemy when the player are in range
    // and check every 0.5s
    [Header("Aggro Settings")]
    [SerializeField] private float _aggroRange = 15f;
    [SerializeField] private float _aggroCheckInterval = 0.5f; 

    private bool _hasAggro; 
    private float _nextAggroCheckTime;

    [Header("Enemy Health Settings")]
    [SerializeField] private EnemyHealth _enemyHealth;
    
    [Header("Enemy Attack Settings")] 
    [SerializeField] private EnemyDamage _enemyAttack;
    
    [SerializeField] private LootEssence EssencePrefab; // Hier ziehst du dein neues Prefab rein!
    [SerializeField] private Rigidbody _rigidbodyPrefab; // Hier ziehst du dein neues Rigidbody Prefab rein!
    
    private Enemysensor _enemysensor;
    private PlayerController _playerController;
    private Rigidbody _rigidbody;
    
    private static ObjektPool<LootEssence> _lootPool;
    
    private void Awake()
    {
        if (_lootPool == null)
        {
            _lootPool = new ObjektPool<LootEssence>(EssencePrefab, _rigidbodyPrefab);
        }
        _rigidbody = GetComponent<Rigidbody>();
        _enemyHealth.Init(gameObject, _lootPool);
        _targetRotation = _rigidbody.rotation;
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        GameObject enmysenserObj = GameObject.Find("EnemySensor");
        if (playerObj != null || enmysenserObj != null)
        {
            _enemysensor = enmysenserObj.GetComponent<Enemysensor>();
            _playerController = playerObj.GetComponent<PlayerController>();
        }
        
    }

    private void Update()
    {
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
    /// <summary>
    /// Check if Player is in Range
    /// </summary>
    private void CheckAggroRange() 
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _playerController.transform.position);
        _hasAggro = distanceToPlayer <= _aggroRange;
        _nextAggroCheckTime = Time.time + _aggroCheckInterval;
    }
    /// <summary>
    /// Get direction to Player
    /// </summary>
    private void GetTargetDirection()
    {
         Vector3 _playerPosition = _playerController.transform.position;
         Vector3 _currentPosition = transform.position;
        _targetDirection = (_playerPosition - _currentPosition).normalized;
        _targetDirection = new Vector3(_targetDirection.x, 0, _targetDirection.z);
        
    }

    private void FixedUpdate()
    {
        // smooth rotation as in PlayerController
        _rigidbody.MoveRotation(Quaternion.Slerp(_rigidbody.rotation, _targetRotation,
                _turnSpeed * Time.fixedDeltaTime));
        if (_hasAggro && !_enemysensor.IsInrange) // only move when the player is in range
        {
            Vector3 velocity = VelocityCalc(_moveSpeed);
            _rigidbody.linearVelocity = velocity;
        }
        else
        {
            _rigidbody.linearVelocity = new Vector3(0, _rigidbody.linearVelocity.y, 0); 
        }
    }
    /// <summary>
    /// Calc the Viloity of the Player
    /// </summary>
    private Vector3 VelocityCalc(float speed)
    {
        Vector3 velocity = _targetDirection * speed;
        velocity.y = _rigidbody.linearVelocity.y;
        return velocity;
    }
    // if a body with the tag "Damage" or "Player" do shit
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

        if (other.CompareTag("Player"))
        {
            if (_enemyAttack.CanDealDamage())
            {
                _playerController.TakeDamage(_enemyAttack.Damage);
            }
        }
    }
    // also if the Player stays in enemy try hitting them
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_enemyAttack.CanDealDamage())
            {
                _playerController.TakeDamage(_enemyAttack.Damage);
            }
        }
    }
}
