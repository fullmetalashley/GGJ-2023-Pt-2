using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    //List of sentences to display in the current dialogue.
    private Queue<string> sentences;
    public Dialogue currentDialogue;
    public bool dialogueActive;
    public bool loadNextScene;

    [Header("Animators")]
    public Animator animator;
    public Animator choiceAnimator;


    [Header("UI Elements")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public GameObject blockerDialogue;
    public GameObject blockerChoices;

    [Header("Choices Menu")]
    public GameObject choices;
    public List<GameObject> choiceButtons;
    public List<TextMeshProUGUI> choiceText;

    [Header("Scene Fade")] public Animator sceneFade;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialogueActive = true;
        currentDialogue = dialogue;

        animator.SetBool("isOpen", true);
        choiceAnimator.SetBool("isOpen", false);
        blockerDialogue.SetActive(true);

        nameText.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            //If we have children, we need to show the choices menu.
            if (currentDialogue.choices != null)
            {
                choices.SetActive(true);
                choiceAnimator.SetBool("isOpen", true);
                blockerChoices.SetActive(true);
                //Set the buttons
                for (int i = 0; i < currentDialogue.children.Length; i++)
                {
                    choiceButtons[i].SetActive(true);
                    choiceText[i].text = currentDialogue.choices[i];
                }

                for (int j = currentDialogue.children.Length; j < choiceButtons.Count; j++)
                {
                    choiceButtons[j].SetActive(false);
                    choiceText[j].text = "";
                }
                return;
            }
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    //Called when a choice is selected. We then load the response for that choice.
    public void ChoiceClick(int index)
    {
        //Turn the choices off. 
        choices.SetActive(false);
        blockerChoices.SetActive(false);
        choiceAnimator.SetBool("isOpen", false);

        //Get the response for that choice, and show dialogue with that.
        //We need to access the value at that index, and find the key for it. 
        //We're currently on the dialogue that tells us the children. 
        //So our new dialogue is whatever has the key for the child of that index. 
        ProcessNextDialogue(index);
    }


    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void ProcessNextDialogue(int index)
    {
        if (currentDialogue.children != null)
        {
            string newKey = currentDialogue.children[index];
            Dialogue nextDialogue = FindObjectOfType<DialogueLoader>().SetDialogue(newKey);

            currentDialogue = nextDialogue;
            StartDialogue(nextDialogue);
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        //We need to do a check for children. 
        if (currentDialogue.children != null)
        {
            //There's a new piece in the chain, and we need to process it.
            ProcessNextDialogue(0);
        }
        else
        {
            currentDialogue = null;
            animator.SetBool("isOpen", false);
            blockerDialogue.SetActive(false);
            dialogueActive = false;

            if (loadNextScene)
            {
                StartCoroutine(DelaySceneLoad());
            }
        }
    }

    //Utilize this when moving to a new scene. Might not want it to be auto though. 
    IEnumerator DelaySceneLoad()
    {
        sceneFade.SetBool("changeScene", true);

        yield return new WaitForSeconds(2f);
        this.GetComponent<LoadScene>().Load();
    }
}