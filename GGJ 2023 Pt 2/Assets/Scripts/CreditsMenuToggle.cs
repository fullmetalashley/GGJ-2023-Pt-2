using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMenuToggle : MonoBehaviour
{
    public GameObject creditsMenu;

    public void ToggleMenu()
    {
        creditsMenu.SetActive(!creditsMenu.activeSelf);
    }
}
