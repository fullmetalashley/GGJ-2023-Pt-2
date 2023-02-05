using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDetection : MonoBehaviour
{
    private DialogueManager theDialogue;
    private UIManager theUI;
    private DialogueTrigger trigger;
    
    public void Start()
    {
        theDialogue = FindObjectOfType<DialogueManager>();
        trigger = this.gameObject.GetComponent<DialogueTrigger>();
        theUI = FindObjectOfType<UIManager>();
    }
    
    void OnMouseDown()
    {
        if (theDialogue.dialogueActive) return;
        if (theUI.uiOpen) return;
        
        if (trigger != null)
        {
            trigger.TriggerDialogue();
        }
        else
        {
            Debug.LogError("No dialog attached to this object!");
        }
    }
}
