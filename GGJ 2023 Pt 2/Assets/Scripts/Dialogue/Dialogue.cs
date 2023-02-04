using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue Object")]
public class Dialogue: ScriptableObject
{
    [Header("Content")]
    public string name;
    public string[] sentences;
    public string[] choices;

    [Header("Key Data")]
    public string dialogueKey;  //Matches up with NPC key
    public string parent;   //Tells who this key belongs to. Might be unnecessary
    public string[] children;   //Tells who the next children are for this key

    [Header("Stat Adjustment")]
    public string[] stat;
    public int[] statValue;

    public Dialogue(string _key, string _parent, string[] _children, string _name,
        string[] _sentences, string[] _choices)
    {
        //Set key data
        dialogueKey = _key;
        parent = _parent;
        children = _children;

        //Set content data
        name = _name;
        sentences = _sentences;
        choices = _choices;

        if (_parent == "null")
        {
            parent = null;
        }
    }
}
