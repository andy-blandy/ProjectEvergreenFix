using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI textbox;
    public Queue<string> sentences;
    public DialogueTrigger dt;

    [Header("Characters")]
    public GameObject Lily;
    public GameObject Alexandra;
    public GameObject Cassius;

    [Header("UI")]
    public GameObject nextButton;
    public GameObject dialogueBox;

    [Header("Audio")]
    public AudioSource buttonSFX;

    void Start()
    {
        Lily.SetActive(false);
        Cassius.SetActive(false);
        sentences = new Queue<string>();
        StartDialogue(dt.GetDialogue());
    }

    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("Starting intro");
        sentences.Clear();
        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        NextSentence();
    }

    public void NextSentence()
    {
        buttonSFX.Play();
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
        dialogueBox.SetActive(false);
        Alexandra.SetActive(false);
        Debug.Log("End of intro");
    }
}
