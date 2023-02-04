using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseUpdate : MonoBehaviour
{
    private DialogueLoader database;

    // Start is called before the first frame update
    void Start()
    {
        database = FindObjectOfType<DialogueLoader>();
        database.SetAllTriggers();
    }
}