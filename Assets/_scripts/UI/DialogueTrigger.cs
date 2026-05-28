using UnityEngine;
using UnityEngine.AdaptivePerformance;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private Dialogue dialogueScript;
    private void OnTriggerEnter(Collider other)
    {
        // irgwo noch disablen dass player weiter gehen kann oder wir lassen das so und bewegen uns bei der präsi eif nd
        if (other.CompareTag("Player"))
        {
            if (dialogueCanvas != null)
            {
                dialogueCanvas.SetActive(true);
            }
            if (dialogueScript != null)
            {
                dialogueScript.TriggerDialogue();
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (dialogueCanvas != null)
            {
                dialogueCanvas.SetActive(false);
            }
        }
    }
}
