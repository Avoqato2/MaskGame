using System.Collections;
using UnityEngine;

// enum for the different mask types, so we can easily switch, public so it can be used in other scripts like the UI
public enum MaskType
{
    None,
    Dash,
    Attack,
	Shield
}
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpSpeed;
    
    [Header("Health Settings")]
    [SerializeField] private float _maxHealth = 100f;
    private float _currentHealth;
    
	[Header("Mask Settings")]
	[SerializeField]
	private MaskType _currentMask = MaskType.None;
    [SerializeField] private PlayerUI _playerUI;

    [Header("Dash Settings")]
    [SerializeField] private float _dashSpeed = 20f;
    [SerializeField] private float _dashTime = 0.25f;
    [SerializeField] private float _dashCooldown = 1.5f;
    
    [Header ("Shield Settings")]
    [SerializeField] private float _shieldTime = 2f;
    [SerializeField] private float _shieldCooldown = 3f;
    
    [Header("Rotation Settings")]
    [SerializeField] private float _rotationSpeed = 5f;
    
    private Quaternion _targetRotation;
    private Vector3 _currentMovementInput;
    private PlayerInputController _playerInputController;
    private GroundController _groundController;
    private Rigidbody _rigidbody;
    private bool _jumpTriggered;
    private bool _dashTriggered;
    private bool _shieldTriggered;
    private bool _isInvincible;
    private float _nextDashTime;
    private float _nextShieldTime;
    
    private int _jumpsLeft = 1;
    

    private void Awake()
    {   //Initiate the Components you need
        _playerInputController = GetComponent<PlayerInputController>();
        _groundController = GetComponent<GroundController>();
        _rigidbody = GetComponent<Rigidbody>();
        _targetRotation = _rigidbody.rotation;
        //Subscribe the Input events you need
        _playerInputController.OnJumpButtonPressed += JumpButtonPressed;
		_playerInputController.OnCycleMaskButtonPressed += CycleMaskButtonPressed;
		_playerInputController.OnExecuteMaskAbilityButtonPressed += ExecuteMaskAbilityButtonPressed;
        
        _currentHealth = _maxHealth;
    }

    private void Update()
    {
        if(_dashTriggered)return; // Dont rotate on dash
        
        //safe the Movement direction
        _currentMovementInput = new Vector3(_playerInputController.MovementInputVector.x, 0f, _playerInputController.MovementInputVector.y).normalized;
        if (_currentMovementInput != Vector3.zero) // "null" exeption
        {
            _targetRotation = Quaternion.LookRotation(_currentMovementInput);
        }
    }
    private void FixedUpdate()
    {
        if(_dashTriggered)return; //Dont touch my Rigidbody while Dashing couse you stink
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
	
	private void CycleMaskButtonPressed()
    {
        int amountOfMasks = System.Enum.GetValues(typeof(MaskType)).Length; // get the number of masks in the enum, so we can loop through them
        int nextMaskIndex = ((int)_currentMask + 1) % amountOfMasks; // modolo weil so fängts wieder von vorne an, wenn du durch alle Masken durch bist
        _currentMask = (MaskType)nextMaskIndex; // cast the index back to the MaskType enum, wollen ja keine int sondern ein MaskType
        Debug.Log("Current Mask: " + _currentMask);
    }

	private void ExecuteMaskAbilityButtonPressed()
    {
        // der code ist die doku, ich hasse kommentare schreiben
        switch (_currentMask)
        {
            case MaskType.None:
                Debug.Log("no mask equipped");
                break;
            case MaskType.Dash:
                if (!_dashTriggered && Time.time > _nextDashTime)
                {
                    StartCoroutine(Dash());
                }
                break;
            case MaskType.Attack:
                PerformAttackPlaceholder();
                break;
            case MaskType.Shield:
                if (!_shieldTriggered && Time.time > _nextShieldTime)
                {
                    StartCoroutine(Shield());
                }
                break;
        }
    }
    private void PerformAttackPlaceholder() { Debug.Log("atttaaack"); }

    private IEnumerator Dash()
    {
        _dashTriggered = true; 
        Vector3 dashDirection = _currentMovementInput;// give me direction please
        if (dashDirection == Vector3.zero) 
        {
            dashDirection = transform.forward; // if u have no direction please just give me the forward direction
        }
        float startTime = Time.time;
        while (Time.time < startTime + _dashTime)
        {
            Vector3 dashVelocity = dashDirection * _dashSpeed;
            dashVelocity.y = _rigidbody.linearVelocity.y; // gravity and shit
            _rigidbody.linearVelocity = dashVelocity;
            yield return new WaitForFixedUpdate(); // Wait till the physics engine says okey
        }
        _nextDashTime = Time.time + _dashCooldown;
        _dashTriggered = false; // stop
    }
    
    private IEnumerator Shield()
    {
        _shieldTriggered = true;
        _isInvincible = true;
        Debug.Log("nooooo damage for me");
        float startTime = Time.time;

        while (Time.time < startTime + _shieldTime)
        {
            yield return new WaitForFixedUpdate();
        }
        
        _nextShieldTime = Time.time + _shieldCooldown;
        _isInvincible = false;
        _shieldTriggered = false;
        
        // das gehört irgwie ausgelagert weil damage health stuff sollte hier ja nicht rein
    }
}
