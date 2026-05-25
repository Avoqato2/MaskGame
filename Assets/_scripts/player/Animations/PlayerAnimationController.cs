using UnityEngine;
using System;

[Serializable]
public class PlayerAnimationController
{
    private Animator _animator;
    private PlayerController _playerController;
    private PlayerInputController _playerInputController;
    private GroundController _groundController;
    
    // pls look up how the Animator in the Animatior Window.
    // There u can see how the StateMachine for the Animations for the Player are connected.
    // Animationscript also extends to ResetAttackBehaviour.cs

    public void Init(Animator animator, PlayerController playerController, 
        PlayerInputController playerInputController, GroundController groundController)
    {
        //ini for Componets
        _animator = animator;
        _playerController = playerController;
        _playerInputController = playerInputController;
        _groundController = groundController;
        // ini for events
        _playerController.OnJumpPerformed += TriggerJumpAnimation;
        _playerInputController.OnAttackButtonPressed += TriggerAttackAnimation;
    }

    public void UpdateAnimations()
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

    public void Cleanup()
    {
        if (_playerController != null)
        {
            _playerController.OnJumpPerformed -= TriggerJumpAnimation;
            _playerInputController.OnAttackButtonPressed -= TriggerAttackAnimation;
        }
    }
}