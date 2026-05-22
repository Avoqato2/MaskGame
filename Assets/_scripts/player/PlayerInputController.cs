using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    // this Inputsystem relies on the file named Player
    public Vector2 MovementInputVector { get; private set; }
    public event Action OnJumpButtonPressed;
    public event Action OnCycleMaskButtonPressed;
    public event Action OnExecuteMaskAbilityButtonPressed;

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
    
    private void OnExecuteMaskAbility(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            OnExecuteMaskAbilityButtonPressed?.Invoke();
        }
    }
}
