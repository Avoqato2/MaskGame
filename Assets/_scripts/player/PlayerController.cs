using UnityEngine;
public class PlayerController : MonoBehaviour
{
    [SerializeField] 
    private float _speed;
    [SerializeField]
    private float _jumpSpeed;
    [SerializeField]
    private float _rotationSpeed = 5f; 
    private Quaternion _targetRotation;
    private Vector3 _currentMovementInput;
    private PlayerInputController _playerInputController;
    private GroundController _groundController;
    private Rigidbody _rigidbody;
    private bool _jumpTriggered;
    private int _jumpsLeft = 1;

    private void Awake()
    {
        _playerInputController = GetComponent<PlayerInputController>();
        _groundController = GetComponent<GroundController>();
        _rigidbody = GetComponent<Rigidbody>();
        _targetRotation = _rigidbody.rotation;
        
        _playerInputController.OnJumpButtonPressed += JumpButtonPressed;
    }

    private void Update()
    {
        _currentMovementInput = new Vector3(_playerInputController.MovementInputVector.x, 0f, _playerInputController.MovementInputVector.y).normalized;
        if (_currentMovementInput != Vector3.zero)
        {
            _targetRotation = Quaternion.LookRotation(_currentMovementInput);
        }
    }
    private void FixedUpdate()
    {
        
        _rigidbody.MoveRotation(Quaternion.Slerp(_rigidbody.rotation, _targetRotation, _rotationSpeed * Time.fixedDeltaTime));
        
        Vector3 velocity = new Vector3(_playerInputController.MovementInputVector.x, 0, _playerInputController.MovementInputVector.y)* _speed;
        velocity.y = _rigidbody.linearVelocity.y;
        if(_jumpTriggered)
        {
            velocity.y = _jumpSpeed;
            _jumpTriggered = false;
        }
        _rigidbody.linearVelocity = velocity;
    }
    
    private void JumpButtonPressed()
    {
        if (_groundController.IsGrounded)
        {
            _jumpTriggered = true;
            _jumpsLeft = 1;
        } else if (_jumpsLeft > 0)
        {
            _jumpsLeft--;
            _jumpTriggered = true;
        }
    }
}
