using UnityEngine;

public enum MaskType
{
    None,
    Dash,
    Attack,
	Block
}
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] 
    private float _speed;
    [SerializeField]
    private float _jumpSpeed;
    [SerializeField]
    private float _rotationSpeed = 5f; 
    
    [Header("Health Settings")]
    [SerializeField] private float _maxHealth = 100f;
    private float _currentHealth;
    
	[Header("Mask Settings")]
	[SerializeField]
	private MaskType _currentMask = MaskType.None;
    [SerializeField] private PlayerUI _playerUI;

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
		_playerInputController.OnCycleMaskButtonPressed += CycleMaskButtonPressed;
		_playerInputController.OnExecuteMaskAbilityButtonPressed += ExecuteMaskAbilityButtonPressed;
        
        _currentHealth = _maxHealth;
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
	
	private void CycleMaskButtonPressed()
    {
        int nextMaskIndex = ((int)_currentMask + 1) % System.Enum.GetValues(typeof(MaskType)).Length;
        _currentMask = (MaskType)nextMaskIndex;
        Debug.Log("Current Mask: " + _currentMask);
    }

	private void ExecuteMaskAbilityButtonPressed()
    {
        switch (_currentMask)
        {
            case MaskType.None:
                Debug.Log("no mask equipped");
                break;
            case MaskType.Dash:
                PerformDashPlaceholder();
                break;
            case MaskType.Attack:
                PerformAttackPlaceholder();
                break;
            case MaskType.Block:
                PerformBlockPlaceholder();
                break;
        }
    }
    
    private void PerformDashPlaceholder() { Debug.Log("zoooom dash"); }
    private void PerformAttackPlaceholder() { Debug.Log("atttaaack"); }
    private void PerformBlockPlaceholder() { Debug.Log("block block"); }
}
