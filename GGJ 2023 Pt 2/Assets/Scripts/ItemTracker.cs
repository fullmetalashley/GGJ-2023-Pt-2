using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTracker : MonoBehaviour
{
    public List<ClickDetection> allItems;

    public Color dimColor;

    public bool presentRoom;

    public DialogueTrigger seeFamilyTree;
    public GameObject familyTree;

    public GameObject falseBottom;

    //Goes through the list, turns all their outlines all, makes them all clickable.
    public void AllItemsOn()
    {
        for (int i = 0; i < allItems.Count; i++)
        {
            allItems[i].gameObject.GetComponent<Outline>().enabled = true;
        }
    }
    
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
        //For the present room, we turn on the journal.
        if (presentRoom)
        {
            //TODO: Turn on the family tree and run family tree dialogue
            //TODO: Stagger dialogue, so make a system where one can be pending until another is finished
            familyTree.GetComponent<MeshRenderer>().enabled = true;
            familyTree.GetComponent<ClickDetection>().enabled = true;
        }
        else
        {
            //We're in the past
            //We need to trigger the cipher object, which will trigger the rest.
            falseBottom.GetComponent<MeshRenderer>().enabled = true;
            falseBottom.GetComponent<ClickDetection>().enabled = true;
        }
    }

    public void ToggleColliders()
    {
        for (int i = 0; i < allItems.Count; i++)
        {
            allItems[i].clickLocked = !allItems[i].clickLocked;
        }
    }
}
