using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _animator;
    private PlayerController _playerController;
    private PlayerInputController _playerInputController;
    private GroundController _groundController;
    

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _playerController = GetComponent<PlayerController>();
        _playerInputController = GetComponent<PlayerInputController>();
        _groundController = GetComponent<GroundController>();
        
        _playerController.OnJumpPerformed += TriggerJumpAnimation;
        _playerInputController.OnAttackButtonPressed += TriggerAttackAnimation;
    }

    private void Update()
    {
        bool isRunning = _playerController.CurrentMovementInput != Vector3.zero;
        _animator.SetBool("Running", isRunning);
        if (!isRunning)
        {
            _playerController.attackTriggered = false;
        }

        if (_groundController != null)
        {
            _animator.SetBool("IsGrounded", _groundController.IsGrounded);
        }
    }

    private void TriggerJumpAnimation()
    {
        _animator.SetTrigger("Jump");
    }
    private void TriggerAttackAnimation()
    {
        _animator.SetTrigger("Punch");
    }

    private void OnDestroy()
    {
        if (_playerController != null)
        {
            _playerController.OnJumpPerformed -= TriggerJumpAnimation;
            _playerInputController.OnAttackButtonPressed -= TriggerAttackAnimation;
        }
    }
}