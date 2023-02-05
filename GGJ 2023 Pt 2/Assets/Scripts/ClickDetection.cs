using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDetection : MonoBehaviour
{
    private DialogueManager theDialogue;
    private UIManager theUI;
    private DialogueTrigger trigger;
    private ItemTracker itemTracker;

    public bool hasBeenClicked;

    public void Start()
    {
        theDialogue = FindObjectOfType<DialogueManager>();
        trigger = this.gameObject.GetComponent<DialogueTrigger>();
        theUI = FindObjectOfType<UIManager>();
        itemTracker = FindObjectOfType<ItemTracker>();

    }
    
    void OnMouseDown()
    {
        if (theDialogue.dialogueActive) return;
        if (theUI.uiOpen) return;
        
        if (trigger != null)
        {
            hasBeenClicked = true;
            DimOutline();
            trigger.TriggerDialogue();
        }
        else
        {
            Debug.LogError("No dialog attached to this object!");
        }
    }

    void DimOutline()
    {
        this.gameObject.GetComponent<Outline>().OutlineColor = itemTracker.dimColor;
        this.gameObject.GetComponent<Outline>().OutlineWidth = .5f;
    }
}
