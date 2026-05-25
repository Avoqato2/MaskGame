using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _animator;
    private PlayerController _playerController;
    private PlayerInputController _playerInputController;
    private GroundController _groundController;
    
    // pls look up how the Animator in the Animatior Window.
    // There u can see how the StateMachine for the Animations for the Player are connected.
    // Animationscript also extends to ResetAttackBehaviour.cs

    private void Awake()
    {
        //ini for Componets
        _animator = GetComponentInChildren<Animator>(); // getting child component (MainCharakter animator)
        _playerController = GetComponent<PlayerController>();
        _playerInputController = GetComponent<PlayerInputController>();
        _groundController = GetComponent<GroundController>();
        // ini for events
        _playerController.OnJumpPerformed += TriggerJumpAnimation;
        _playerInputController.OnAttackButtonPressed += TriggerAttackAnimation;
    }

    private void Update()
    {
        bool isRunning = _playerController.CurrentMovementInput != Vector3.zero; // set running if currentMovement is moving
        _animator.SetBool("Running", isRunning);

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