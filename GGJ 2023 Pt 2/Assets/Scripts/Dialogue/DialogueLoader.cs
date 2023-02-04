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
        
        //Also at this point, search for any auto running dialogue.
    }

    public void SetAllTriggers()
    {
        allTriggers = FindObjectsOfType<DialogueTrigger>();
        foreach (DialogueTrigger trigger in allTriggers)
        {
            
            trigger.SetTrigger();
        }

        foreach (DialogueTrigger trigger in allTriggers)
        {
            if (trigger.autoDialogue)
            {
                //Run this one NOW.
                trigger.AutoRun();
            }
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
            for (int y = 0; y < cleanDummy.Count; y++)
            {
                if (cleanDummy[y].Contains("[["))
                {
                    childDivider = y;
                    break;
                }
            }
            //If childDivider is still 0, that means we do not have children.

            //First, send in the dialogue. Return an array of cleaned up sentences.
            string[] sentences = ProcessDialogueSentences(cleanDummy, childDivider);
            
            string[] children = ProcessChildren(cleanDummy, childDivider, sentences.Length);

            string[] choices = ProcessKeys(cleanDummy, childDivider, sentences.Length);
            
            Dialogue newLog = new Dialogue(cleanDummy[0], cleanDummy[cleanDummy.Count - 1], sentences, choices, children);
            dialogues.Add(newLog);
        }
    }

    //Go through cleanDummy and pull out the dialogue sentences.
    //Return an array of these sentences.
    string[] ProcessDialogueSentences(List<string> cleanDummy, int childDivider)
    {
        //If childDivider is still 0, we have no choices or options. So our final index is the NPC name, and sentences run right up to that.
        //Therefore, the count of our sentences is from index 1 up to the length - 1.
        int finalIndex = cleanDummy.Count - 2;
        
        if (childDivider != 0)
        {
            //If childDivider is not 0, we'll set our final index to match that so we know where to end the sentences.
            finalIndex = childDivider - 1;
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

        List<string> choices = new List<string>();
        
        for (int z = 0; z < merged.Length; z++)
        {
            string[] messyDummyChoices = merged[z].Split("| "[0]);
            messyDummyChoices[1] = messyDummyChoices[1].Replace("]]", "");
            choices.Add(messyDummyChoices[1].Trim());
        }        
        
        return choices.ToArray();
    }

    string[] ProcessKeys(List<string> cleanDummy, int childDivider, int sentenceLength)
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
        List<string> choices = new List<string>();
        
        for (int z = 0; z < merged.Length; z++)
        {
            string[] messyDummyChoices = merged[z].Split("| "[0]);
            messyDummyChoices[0] = messyDummyChoices[0].Replace("[[", "");
            choices.Add(messyDummyChoices[0].Trim());
        }        
        
        return choices.ToArray();
    }

    //Sets a key based on the dialogue that was passed in.
    public Dialogue SetDialogue(string _key)
    {
        for (int i = 0; i < dialogues.Count; i++)
        {
            if (dialogues[i].dialogueKey == _key)
            {
                return dialogues[i];
            }
        }
        return null;
    }
}