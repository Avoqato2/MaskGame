using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI healthBar;
    [SerializeField] private Image maskImage;
    
    [SerializeField] private Sprite[] maskSprites;
    public void UpdateMask(MaskType activeMask)
    {
        if(maskImage == null) return;
        if (activeMask == MaskType.None)
        {
            maskImage.enabled = false;
            return;
        }
        maskImage.enabled = true;
        
        int maskIndex = (int)activeMask; // get the index of the active mask from the enum
        if (maskIndex >= 0 && maskIndex < maskSprites.Length) // check
        {
            maskImage.sprite = maskSprites[maskIndex]; // set the sprite of the mask image to the corresponding sprite in the array
        }
        else
        {
            Debug.LogWarning("Invalid mask index: " + maskIndex);
        }
        
    }
    
    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (healthBar != null)
        {
            healthBar.text = $"Health: {currentHealth}/{maxHealth}";
        }
    }
}
