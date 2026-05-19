using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    // this Inputsystem relies on the file named Player
    public Vector2 MovementInputVector { get; private set; }
    public event Action OnJumpButtonPressed;
    public event Action OnDashButtonPressed;
    public event Action OnMaskAbilityButtonPressed;

    private void OnMove(InputValue inputValue)
    {
        MovementInputVector = inputValue.Get<Vector2>() ;
    }

    private void OnJump(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            OnJumpButtonPressed?.Invoke();
        }
    }

    private void OnDash(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            OnDashButtonPressed?.Invoke();
        }
    }
    
    private void OnMaskAbility(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            OnMaskAbilityButtonPressed?.Invoke();
        }
    }
}
