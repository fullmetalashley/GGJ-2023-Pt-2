using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.IO;
using Debug = UnityEngine.Debug;

public class DialogueLoader : MonoBehaviour
{
    public TextAsset[] keys;    //All keys are dragged into here for now. LoadAll would be nice!
    public List<Dialogue> dialogues;

    private static bool exists;

    public DialogueTrigger[] allTriggers;

    void Awake()
    {
        if (!exists)
        {
            exists = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Start()
    {
        ParseKeys();
        SetAllTriggers();
    }

    public void SetAllTriggers()
    {
        Debug.Log("Setting all triggers");
        allTriggers = FindObjectsOfType<DialogueTrigger>();
        foreach (DialogueTrigger trigger in allTriggers)
        {
            
            trigger.SetTrigger();
        }
    }

    public void ParseKeys()
    {
        dialogues = new List<Dialogue>();

        keys = Resources.LoadAll<TextAsset>("Keys");

        for (int i = 0; i < keys.Length; i++)
        {
            string[] dummy = keys[i].text.Split("\n"[0]);

            //First, let's go through and parse all of the lines. Remove blank ones, and trim existing ones.
            List<string> cleanDummy = new List<string>();
            for (int z = 0; z < dummy.Length; z++)
            {
                if (!System.String.IsNullOrWhiteSpace(dummy[z]))
                {
                    cleanDummy.Add(dummy[z].Trim());
                }
            }

            //THIS IS THE PROCESSING OF THE CHUNK.
            //Index 0 will always be the key. 
            //Index 1 will always be the start of content.
            //The index starting with [[ will mark the beginning of children.
            //The final index will always be the NPC name.

            //If we have children, then we can Process children and Process Keys. But if we don't, we can set those to null automatically. 
            int childDivider = 0;
            for (int y = 0; y < dummy.Length; y++)
            {
                if (dummy[y].Contains("[["))
                {
                    childDivider = y;
                    break;
                }
            }
            //If childDivider is still 0, that means we do not have children.

            //First, send in the dialogue. Return an array of cleaned up sentences.
            string[] sentences = ProcessDialogueSentences(cleanDummy, childDivider);

            string[] children = ProcessChildren(cleanDummy, childDivider, sentences.Length);

            for (int z = 0; z < children.Length; z++)
            {
                Debug.Log(children[z]);
            }

        //string[] choices = ProcessKeys(cleanDummy, childDivider);
            
            
            Dialogue newLog = new Dialogue(dummy[0], dummy[1], children, dummy[3], sentences, children);
            dialogues.Add(newLog);
            
        }
    }

    //Go through cleanDummy and pull out the dialogue sentences.
    //Return an array of these sentences.
    string[] ProcessDialogueSentences(List<string> cleanDummy, int childDivider)
    {
        //If childDivider is still 0, we have no choices or options. So our final index is the NPC name, and sentences run right up to that.
        //Therefore, the count of our sentences is from index 1 up to the length - 1.
        int finalIndex = cleanDummy.Count - 1;
        
        if (childDivider != 0)
        {
            //If childDivider is not 0, we'll set our final index to match that so we know where to end the sentences.
            finalIndex = childDivider;
        }

        string[] sentences = new string[finalIndex];
        for (int i = 0; i < sentences.Length; i++)
        {
            sentences[i] = cleanDummy[i + 1];
        }
        return sentences;
    }

    string[] ProcessChildren(List<string> cleanDummy, int childDivider, int sentenceLength)
    {
        //OKAY SO FIRST. We need to figure out if we're even returning anything. If we have no children, just return null.
        if (childDivider == 0)
        {
            return null;
        }
        
        //Great. Now we start at the end of sentences, and move to the second to last value. These represent all our choices. 
        string[] merged = new string[(cleanDummy.Count - 2 - sentenceLength)];
        
        //Okay, now we need to go through clean dummy from child index, to index-1.
        for (int i = 0; i < merged.Length; i++)
        {
            merged[i] = cleanDummy[childDivider + i];
        }
        
        //Cool. Now we just need to remove the brackets, and add the first part of that to the choices. 
        return merged;
        
/*
            //CLEANING UP THE CHOICES / CHILDREN
            //Splits the dummy choices by character, then cleans them up and adds them back into the correct
            string[] messyDummyChoices = childCombo.Split("]][["[0]);

            List<string> refinedDummy = new List<string>();
            
            for (int t = 0; t < messyDummyChoices.Length; t++)
            {
                if (messyDummyChoices[t] != "\n" && messyDummyChoices[t] != "")
                {
                    messyDummyChoices[t] = messyDummyChoices[t].Replace("[[", "");
                    refinedDummy.Add(messyDummyChoices[t]);
                }
            }

            //Now we should be able to go back through and reprocess.
            string[] cleanDummyChoices = new string[refinedDummy.Count];
            for (int p = 0; p < cleanDummyChoices.Length; p++)
            {
                cleanDummyChoices[p] = refinedDummy[p];
            }

            string[] dummyChoices = new string[cleanDummyChoices.Length];
            string[] dummyChildren = new string[cleanDummyChoices.Length];
            ///Ooooookay, now we need to split clean dummy choices into choices, and children.
            for (int z = 0; z < cleanDummyChoices.Length; z++)
            {
                string[] splitOptions = cleanDummyChoices[z].Split("| "[0]);
                //The first will be the choice, the second will be the child.
                dummyChoices[z] = splitOptions[0];
                dummyChildren[z] = splitOptions[1];
                dummyChildren[z] = dummyChildren[z].Remove(0, 1);    //Remove the extra space they start with
            }
*/
    }

    void ProcessKeys()
    {
        
    }

    void ProcessChoices()
    {
        
    }

    //Sets a key based on the dialogue that was passed in.
    public Dialogue SetDialogue(string _key)
    {
        Debug.Log("Key to check: " + _key);
        for (int i = 0; i < dialogues.Count; i++)
        {
            Debug.Log("Stored key: " + dialogues[i].dialogueKey);
            if (dialogues[i].dialogueKey == _key)
            {
                Debug.Log("found it");
                return dialogues[i];
            }
        }
        return null;
    }
}