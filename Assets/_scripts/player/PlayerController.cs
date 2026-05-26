using System.Collections;
using UnityEngine;
using System;
// Refactoring was vibecoded hehe
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")] 
    [SerializeField] private MovementSettings movement;

    [Header("Health Settings")] 
    [SerializeField] private PlayerHealth _health; //Load PlayerHealth Class
    
    [Header("Ground Tolerance Settings")]
    [SerializeField] private GroundController _groundController; //Load GroundController Class
    
    [Header("Abilities")]
    [SerializeField] private MaskManager _maskManager;//Load MaskManager Class
    
    [SerializeField]private PlayerAnimationController _playerAnimationController;//Load PlayerAnimationController Class

    [Header("UI Settings")]
    [SerializeField] private PlayerUI _playerUI;
    
    private Quaternion _targetRotation;
    private Vector3 _currentMovementInput;
    private PlayerInputController _playerInputController;
    private Rigidbody _rigidbody;
    private bool _jumpTriggered;
    
    public bool AttackTriggered {get; private set;} // maybe for future purposes public now it could be private

    private int _jumpsLeft = 1;

    public Vector3 CurrentMovementInput => _currentMovementInput; // so other scripts can access it (maskmanager)
    public event Action OnJumpPerformed; // For animatorController

    private void Awake()
    {
        //Initiate the Components you need
        _playerInputController = GetComponent<PlayerInputController>();
        _rigidbody = GetComponent<Rigidbody>();
        _targetRotation = _rigidbody.rotation;
        //Subscribe the Input events you need
        _playerInputController.OnJumpButtonPressed += JumpButtonPressed;
        _playerInputController.OnAttackButtonPressed += AttackButtonPressed;
        _playerInputController.OnCycleMaskButtonPressed += _maskManager.CycleMask; // Sent events directly to MaskManager
        _playerInputController.OnExecuteMaskAbilityButtonPressed += _maskManager.ExecuteMaskAbility;
        
        //Give Classes all Variables they need
        _groundController.Init(GetComponent<CapsuleCollider>(), transform);
        _maskManager.PlayerUI = _playerUI;
        if (_maskManager.PlayerUI != null)
        {
            _maskManager.PlayerUI.UpdateMask(_maskManager.CurrentMask);
        }
        _health.Init(_playerUI);
        Animator modelAnimator = GetComponentInChildren<Animator>();
        _playerAnimationController.Init(modelAnimator,this,_playerInputController, _groundController);
    }

    private void Update()
    {
        //if(_dashTriggered)return; // Dont rotate on dash
        // if(_maskManager.DashTriggered) return; // no rotate on dash
        
        //safe the Movement direction
        _currentMovementInput = new Vector3(_playerInputController.MovementInputVector.x, 0f, _playerInputController.MovementInputVector.y).normalized;
        if (_currentMovementInput != Vector3.zero) // "null" exeption
        {
            _targetRotation = Quaternion.LookRotation(_currentMovementInput);
        }
        _playerAnimationController.UpdateAnimations(); // read this line u understand
    }

    private void FixedUpdate()
    {
        _maskManager.UpdateAbilites(_rigidbody,_currentMovementInput,transform);
        if (_maskManager.IsDashing) return; //Dont touch my Rigidbody while Dashing couse you stink
        _groundController.CheckGround();
        // Smooth PlayerRotation
        _rigidbody.MoveRotation(Quaternion.Slerp(_rigidbody.rotation, _targetRotation,  movement.rotationSpeed * Time.fixedDeltaTime));
        
        Vector3 velocity = VelocityCalc(movement.speed);
        if(_jumpTriggered)
        {
            velocity.y = movement.speed;
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
            _jumpsLeft = 1; // set jump to 1 couse u touched the ground
            OnJumpPerformed?.Invoke(); // Do Animation on Jump
        }
        else if (_jumpsLeft > 0) // if u have 1 jump left, jump again
        {
            _jumpsLeft--;
            _jumpTriggered = true;
            OnJumpPerformed?.Invoke();
        }
    }

    private void OnDestroy() // delete stuff for RAM safes
    {
        if (_playerInputController != null)
        {
            _playerInputController.OnJumpButtonPressed -= JumpButtonPressed;
            _playerInputController.OnAttackButtonPressed -= AttackButtonPressed;
            _playerInputController.OnCycleMaskButtonPressed -= _maskManager.CycleMask;
            _playerInputController.OnExecuteMaskAbilityButtonPressed -= _maskManager.ExecuteMaskAbility;
        }
        _playerAnimationController.Cleanup();
    }

    private void AttackButtonPressed()
    {
        AttackTriggered = true; // stoping movement couse ur punching
    }

    public void EndAttack()
    {
        AttackTriggered = false; // Setting movement free again
    }
}

[Serializable]
public class MovementSettings // Just for Aufklappbar fields 
{
    public float speed = 7f;
    public float jumpSpeed = 7f;
    public float rotationSpeed = 5f;
}
