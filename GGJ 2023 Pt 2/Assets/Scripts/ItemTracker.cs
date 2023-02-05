using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTracker : MonoBehaviour
{
    public List<ClickDetection> allItems;

    public Color dimColor;
    
    public void CheckForClicks()
    {
        for (int i = 0; i < allItems.Count; i++)
        {
            if (!allItems[i].hasBeenClicked)
            {
                return;
            }

        }

        //We have clicked everything.
        Debug.Log("All things clicked");
    }
}
