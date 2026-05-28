using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dialogue : MonoBehaviour
{   
    [Header("Dialogue Settings")]
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private PlayerInputController playerInputController;
    public string[] lines;
    public float textSpeed;

    private Coroutine typingCoroutine;
    private bool isDialogueActive;

    private int index;
    
    public void TriggerDialogue()
    {
        isDialogueActive = true;
        index = 0;
        textComponent.text = string.Empty;
        
        playerInputController.OnNextDialoguePressed += NextDialoguePressed;
        
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeLine());
    }

    void NextDialoguePressed()
    {
        Debug.Log("NextDialoguePressed");
        if(!isDialogueActive) return;
        if (textComponent.text == lines[index])
        {
            Debug.Log("hallo halo");
            NextLine();
        }
        else
        {
            StopCoroutine(typingCoroutine);
            textComponent.text = lines[index];
        }
    }
    
    IEnumerator TypeLine()
    {
        textComponent.text = string.Empty;
        // so wird der Text Buchstabe für Buchstabe angezeigt, mit einer kurzen Pause dazwischen, die durch textSpeed bestimmt wird
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
    
    void NextLine()
    {
        // wenn es noch mehr Zeilen gibt, wird die nächste Zeile gestartet, ansonsten wird das Dialogfenster geschlossen
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }
    
    void EndDialogue(){
        isDialogueActive = false;
        textComponent.text = string.Empty;
        transform.parent.gameObject.SetActive(false);
    }
    
    private void OnDestroy()
    {
        // Sicherheits-Cleanup für den RAM
        playerInputController.OnNextDialoguePressed -= NextDialoguePressed;
    }
}
