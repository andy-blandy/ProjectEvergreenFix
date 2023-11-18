using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;
using System.Linq;

public class DialogueManager : MonoBehaviour
{
    [Header("Logic")]
    public bool inDialogue;
    public Dialogue currentDialogue;
    public Queue<string> sentences;
    public DialogueTrigger dt;
    public delegate void FinishDialogue();
    public static event FinishDialogue OnDialogueEnded;

    [Header("Characters")]
    public GameObject Lily;
    public GameObject Alexandra;
    public GameObject Cassius;

    public enum Character { Lily, Alexandra, Cassius };
    private Dictionary<Character, GameObject> characterDict = new Dictionary<Character, GameObject>();

    [Header("UI")]
    public GameObject nextButton;
    public GameObject dialogueBox;
    public TextMeshProUGUI textbox;

    [Header("Audio")]
    public AudioSource buttonSFX;

    public static DialogueManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Lily.SetActive(false);
        Alexandra.SetActive(false);
        Cassius.SetActive(false);

        characterDict = new Dictionary<Character, GameObject>()
        {
        { Character.Lily, Lily },
        { Character.Alexandra, Alexandra },
        { Character.Cassius, Cassius }
        };

        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (inDialogue)
        {
            Debug.Log("Error! Already in dialogue.");
            return;
        }

        Debug.Log("Starting dialogue");

        // Set logic
        inDialogue = true;
        currentDialogue = dialogue;

        // Activate the right game object depending on the character chosen
        if (!characterDict.ContainsKey(dialogue.character))
        {
            Debug.Log("ERROR! That character doesn't exist!");
            return;
        }
        characterDict[dialogue.character].SetActive(true);
        
        // Load sentences
        sentences.Clear();
        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        // Begin dialogue
        dialogueBox.SetActive(true);
        NextSentence();
    }

    public void NextSentence()
    {
        // buttonSFX.Play();

        textbox.text = "";
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        textbox.text = sentence;
    }

    public void EndDialogue()
    {
        // Hide dialogue and character
        dialogueBox.SetActive(false);
        characterDict[currentDialogue.character].SetActive(false);

        // Set logic
        inDialogue = false;
        currentDialogue = null;

        // Call event
        OnDialogueEnded();

        Debug.Log("End of dialogue");
    }
}
