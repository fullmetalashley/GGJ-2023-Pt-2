using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue Object")]
public class Dialogue
{
    [Header("Content")]
    public string name;
    public string[] sentences;
    public string[] choices;

    [Header("Key Data")]
    public string dialogueKey;  //Matches up with NPC key
    public string[] children;   //Tells who the next children are for this key. These are the key values of those children.

    [Header("Stat Adjustment")]
    public string[] stat;
    public int[] statValue;

    //Dialouge key
    //NPC name
    //Sentences
    //Choices
    //Children
    public Dialogue(string _key, string _name, string[] _sentences, string[] _choices, string[] _children)
    {
        //Set key data
        dialogueKey = _key;
        children = _children;

        //Set content data
        name = _name;
        sentences = _sentences;
        choices = _choices;
    }
}
