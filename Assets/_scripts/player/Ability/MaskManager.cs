using UnityEngine;
using System;
// enum for the different mask types, so we can easily switch, public so it can be used in other scripts like the UI
public enum MaskType
{
    None,
    Dash,
    Attack,
    Shield
}
// We like Vibe coding xD
[Serializable]
public class MaskManager 
{ 
    [Header("Mask Settings")]
    public MaskType CurrentMask = MaskType.None;
    public PlayerUI PlayerUI;

    [Header("Dash Settings")]
    [SerializeField] private AbilityDash _abilityDash;
    
    [Header ("Shield Settings")] 
    [SerializeField] private AbilityShield _abilityShield;
    
    [Header("Attack Settings")]
    [SerializeField]private AbilityAttack _abilityAttack;
    
    public bool IsDashing => _abilityDash.IsDashing;
    public bool IsAttacking => _abilityAttack.IsAttacking;
    public bool IsInvincible => _abilityShield.IsInvincible;

    private PlayerController _playerController;

    public void Init(PlayerController playerController)
    {
        _abilityAttack.Init(playerController);
        _abilityDash.Init(playerController);
        _abilityShield.Init(playerController);
    }
    public void CycleMask()
    {
        int amountOfMasks = System.Enum.GetValues(typeof(MaskType)).Length; // get the number of masks in the enum, so we can loop through them
        int nextMaskIndex = ((int)CurrentMask + 1) % amountOfMasks; // modolo weil so fängts wieder von vorne an, wenn du durch alle Masken durch bist
        CurrentMask = (MaskType)nextMaskIndex; // cast the index back to the MaskType enum, wollen ja keine int sondern ein MaskType
        Debug.Log("Current Mask: " + CurrentMask);

        if (PlayerUI != null)
        {
            PlayerUI.UpdateMask(CurrentMask);
        }
    }
    
    public void ExecuteMaskAbility()
    {
        // der code ist die doku, ich hasse kommentare schreiben
        switch (CurrentMask)
        {
            case MaskType.None:
                Debug.Log("no mask equipped");
                break;
            case MaskType.Dash:
                _abilityDash.TryDash();
                break;
            case MaskType.Attack:
                _abilityAttack.TryAttack();
                break;
            case MaskType.Shield:
                _abilityShield.TryShield();
                break;
        }
    }
    
    public void UpdateAbilites(Rigidbody rigidbody, Vector3 movementInput,
        Transform playerTransform)
    {
        _abilityDash.UpdateDash(rigidbody, movementInput, playerTransform);
        _abilityShield.UpdateShield();
        _abilityAttack.UpdateAttack(playerTransform, movementInput);
    }
}
