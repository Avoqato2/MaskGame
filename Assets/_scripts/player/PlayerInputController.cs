using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public Vector2 MovementInputVector { get; private set; }
    public event Action OnJumpButtonPressed;
    public event Action OnCycleMaskButtonPressed;
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
    
    private void OnCycleMask(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            OnCycleMaskButtonPressed?.Invoke();
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
