using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sits on an object and allows new dialogue to be triggered.
public class DialogueTrigger : MonoBehaviour
{
    public string dialogueKey;
    public Dialogue dialogue;

    public void SetTrigger()
    {
        dialogue = FindObjectOfType<DialogueLoader>().SetDialogue(dialogueKey);
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}