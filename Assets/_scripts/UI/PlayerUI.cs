using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _healthBar;
    [SerializeField] private Image _maskImage;
    
    [SerializeField] private Sprite[] _maskSprites;
    public void UpdateMask(MaskType activeMask)
    {
        if(_maskImage == null) return;
        if (activeMask == MaskType.None)
        {
            _maskImage.enabled = false;
            return;
        }
        _maskImage.enabled = true;
        
        int maskIndex = (int)activeMask; // get the index of the active mask from the enum
        if (maskIndex >= 0 && maskIndex < _maskSprites.Length) // check
        {
            Debug.Log("Setting sprite: " + _maskSprites[maskIndex]);
            _maskImage.sprite = _maskSprites[maskIndex]; // set the sprite of the mask image to the corresponding sprite in the array
        }
        else
        {
            Debug.LogWarning("Invalid mask index: " + maskIndex);
        }
        
    }
    
    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (_healthBar != null)
        {
            _healthBar.text = $"Health: {currentHealth}/{maxHealth}";
        }
    }
}
