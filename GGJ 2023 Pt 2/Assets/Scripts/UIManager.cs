using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject letter;
    public bool uiOpen;

    public GameObject endScreen;
    
    public void ToggleLetter()
    {
        letter.GetComponent<DialogueTrigger>().TriggerDialogue();
        letter.SetActive(!letter.activeSelf);
        uiOpen = letter.activeSelf;
    }

    public void EndGame()
    {
        endScreen.SetActive(true);
    }
}
