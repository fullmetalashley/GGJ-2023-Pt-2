using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject letter;
    public bool uiOpen;

    public void ToggleLetter()
    {
        letter.SetActive(!letter.activeSelf);
        uiOpen = letter.activeSelf;
    }
}
