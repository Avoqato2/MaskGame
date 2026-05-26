using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;

    private int index;
    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }
    
    void Update()
    {
        if(Keyboard.current != null && Keyboard.current.jKey.wasPressedThisFrame)
        {
            if (textComponent.text == lines[index])
            {
                Debug.Log("hallo halo");
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }
    
    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }
    
    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
    
    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
