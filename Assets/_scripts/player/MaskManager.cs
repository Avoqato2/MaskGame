using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

// enum for the different mask types, so we can easily switch, public so it can be used in other scripts like the UI
public enum MaskType
{
    None,
    Dash,
    Attack,
    Shield
}
public class MaskManager : MonoBehaviour
{
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
    
    private PlayerInputController _playerInputController;
    private PlayerController _playerController;
    private Rigidbody _rigidbody;
    
    private bool _dashTriggered;
    private bool _shieldTriggered;
    private bool _isInvincible;
    private float _nextDashTime;
    private float _nextShieldTime;
    
    public bool DashTriggered => _dashTriggered;
    public bool IsInvincible => _isInvincible;
    private void Awake()
    {
        _playerInputController = GetComponent<PlayerInputController>();
        _playerController = GetComponent<PlayerController>();
        _rigidbody = GetComponent<Rigidbody>();
        
        _playerInputController.OnCycleMaskButtonPressed += CycleMaskButtonPressed;
        _playerInputController.OnExecuteMaskAbilityButtonPressed += ExecuteMaskAbilityButtonPressed;
    }
    
    private void CycleMaskButtonPressed()
    {
        int amountOfMasks = System.Enum.GetValues(typeof(MaskType)).Length; // get the number of masks in the enum, so we can loop through them
        int nextMaskIndex = ((int)_currentMask + 1) % amountOfMasks; // modolo weil so fängts wieder von vorne an, wenn du durch alle Masken durch bist
        _currentMask = (MaskType)nextMaskIndex; // cast the index back to the MaskType enum, wollen ja keine int sondern ein MaskType
        Debug.Log("Current Mask: " + _currentMask);
        
        //todo: ui info update
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
    
    private IEnumerator Dash()
    {
        _dashTriggered = true; 
        Vector3 dashDirection = _playerController.CurrentMovementInput;// give me direction please
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
    
    private void PerformAttackPlaceholder() { Debug.Log("atttaaack"); }
}
