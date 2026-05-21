using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _healthBar;
    [SerializeField] private TextMeshProUGUI _maskDisplay;
    
    //später wenn bilder für mask da sind
    [SerializeField] private Sprite[] _maskSprites;
    public void UpdateMask(MaskType activeMask)
    {
        if(_maskDisplay != null)
        {
            _maskDisplay.text = activeMask.ToString();
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
