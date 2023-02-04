using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Sits on an object and allows new dialogue to be triggered.
public class DialogueTrigger : MonoBehaviour
{
    public string dialogueKey;
    public Dialogue dialogue;

    public bool autoDialogue;

    public float delayTime;

    public bool sceneTrigger;

    private DialogueManager theDialogue;

    void Awake()
    {
        theDialogue = FindObjectOfType<DialogueManager>();
    }
    
    public void SetTrigger()
    {
        dialogue = FindObjectOfType<DialogueLoader>().SetDialogue(dialogueKey);
    }

    public void TriggerDialogue()
    {
        theDialogue.StartDialogue(dialogue);
        theDialogue.loadNextScene = sceneTrigger;
    }

    public void AutoRun()
    {
        StartCoroutine(AutoDialogue());
    }
    
    IEnumerator AutoDialogue()
    {
        yield return new WaitForSeconds(delayTime);
        if (delayTime == 0)
        {
            Debug.LogError("Set the delay time for this auto message in the inspector!");
        }
        TriggerDialogue();
    }
}