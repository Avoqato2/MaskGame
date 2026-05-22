using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpSpeed;
    
    [Header("Health Settings")]
    [SerializeField] private float _maxHealth = 100f;
    private float _currentHealth;
    
    [Header("Rotation Settings")]
    [SerializeField] private float _rotationSpeed = 5f;
    
    private Quaternion _targetRotation;
    private Vector3 _currentMovementInput;
    private PlayerInputController _playerInputController;
    private GroundController _groundController;
    private MaskManager _maskManager; // now a separate script
    private Rigidbody _rigidbody;
    private bool _jumpTriggered;
    
    private int _jumpsLeft = 1;

    public Vector3 CurrentMovementInput => _currentMovementInput; // so other scripts can access it (maskmanager)
    
    private void Awake()
    {   //Initiate the Components you need
        _playerInputController = GetComponent<PlayerInputController>();
        _groundController = GetComponent<GroundController>();
        _maskManager = GetComponent<MaskManager>();
        _rigidbody = GetComponent<Rigidbody>();
        _targetRotation = _rigidbody.rotation;
        //Subscribe the Input events you need
        _playerInputController.OnJumpButtonPressed += JumpButtonPressed;
        
        _currentHealth = _maxHealth;
    }

    private void Update()
    {
        //if(_dashTriggered)return; // Dont rotate on dash
        if(_maskManager.DashTriggered) return; // no rotate on dash
        
        //safe the Movement direction
        _currentMovementInput = new Vector3(_playerInputController.MovementInputVector.x, 0f, _playerInputController.MovementInputVector.y).normalized;
        if (_currentMovementInput != Vector3.zero) // "null" exeption
        {
            _targetRotation = Quaternion.LookRotation(_currentMovementInput);
        }
    }
    private void FixedUpdate()
    {
        //if(_dashTriggered)return; //Dont touch my Rigidbody while Dashing couse you stink
        if(_maskManager.DashTriggered) return;
        // Smooth PlayerRotation
        _rigidbody.MoveRotation(Quaternion.Slerp(_rigidbody.rotation, _targetRotation, _rotationSpeed * Time.fixedDeltaTime));
        
        Vector3 velocity = VelocityCalc(_speed);
        if(_jumpTriggered)
        {
            velocity.y = _jumpSpeed;
            _jumpTriggered = false;
        }

        _rigidbody.linearVelocity = velocity;
    }

    private Vector3 VelocityCalc(float speed)
    {
        //Calc of Player Velocity
        Vector3 velocity = new Vector3(_playerInputController.MovementInputVector.x, 0, _playerInputController.MovementInputVector.y)* speed;
        velocity.y = _rigidbody.linearVelocity.y; // Set Player Y-Access to currentLocation
        return velocity;
    }
    
    private void JumpButtonPressed()
    {
        if (_groundController.IsGrounded)
        {
            _jumpTriggered = true;
            _jumpsLeft = 1;// set jump to 1 couse u touched the ground
        } else if (_jumpsLeft > 0) // if u have 1 jump left, jump again
        {
            _jumpsLeft--;
            _jumpTriggered = true;
        }
    }
}
