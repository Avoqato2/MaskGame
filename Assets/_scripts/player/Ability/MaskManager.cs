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
    public float DashSpeed = 20f;
    public float DashTime = 0.25f;
    public float DashCooldown = 1.5f;
    
    [Header ("Shield Settings")] 
    public float ShieldTime = 2f;
    public float ShieldCooldown = 3f;
    
    [Header("Attack Settings")]
    [SerializeField]private AbilityAttack _abilityAttack;

    private PlayerController _playerController;
    public bool IsDashing { get; private set; }
    private float _dashEndTime;
    private float _nextDashTime;

    public bool IsInvincible { get; private set; }
    private float _shieldEndTime;
    private float _nextShieldTime;

    public void Init(PlayerController playerController)
    {
        _abilityAttack.Init(playerController);
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
                TryDash();
                break;
            case MaskType.Attack:
                _abilityAttack.TryAttack();
                break;
            case MaskType.Shield:
                TryShield();
                break;
        }
    }
    private void TryDash()
    {
        if (!IsDashing && Time.time >= _nextDashTime)
        {
            IsDashing = true;
            _dashEndTime = Time.time + DashTime;
        }
    }

    private void TryShield()
    {
        if (!IsInvincible && Time.time >= _nextShieldTime)
        {
            IsInvincible = true;
            _shieldEndTime = Time.time + ShieldTime;
            Debug.Log("nooooo damage for me");
        }
    }

    public void UpdateAbilites(Rigidbody rigidbody, Vector3 movementInput,
        Transform playerTransform)
    {
        UpdateDash(rigidbody, movementInput, playerTransform);
        UpdateShield();
        _abilityAttack.UpdateAttack(playerTransform, movementInput);
    }
    private void UpdateDash(Rigidbody rigidbody, Vector3 movementInput,
        Transform playerTransform)
    {
        if(!IsDashing)return;
        if (Time.time >= _dashEndTime)
        {
            IsDashing = false;
            _nextDashTime = Time.time + DashCooldown;
            return;
        }
        
        Vector3 dashDirection = movementInput;
        if (dashDirection == Vector3.zero)
        {
            dashDirection = playerTransform.forward;
        }
        Vector3 dasVelocity = dashDirection * DashSpeed;
        dasVelocity.y = rigidbody.linearVelocity.y;
        rigidbody.linearVelocity = dasVelocity;
    }
    
    private void UpdateShield()
    {
        if(!IsInvincible) return;
        if (Time.time >= _shieldEndTime)
        {
            IsInvincible = false;
            _nextShieldTime = Time.time + ShieldCooldown;
            Debug.Log("Shield deactivated!");
        }
        // das gehört irgwie ausgelagert weil damage health stuff sollte hier ja nicht rein
    }
}
