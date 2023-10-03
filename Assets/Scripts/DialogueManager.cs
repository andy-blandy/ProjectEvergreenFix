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
    public GameObject Lily;
    public GameObject Alexandra;
    public GameObject Cassius;
    public DialogueTrigger dt;
    public GameObject nextButton;
    // Start is called before the first frame update
    void Start()
    {
        Lily.SetActive(false);
        Cassius.SetActive(false);
        sentences = new Queue<string>();
        startDialogue(dt.getDialogue());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void startDialogue(Dialogue dialogue)
    {
        Debug.Log("Starting intro");
        sentences.Clear();
        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        nextSentence();
    }
    public void nextSentence()
    {
        textbox.text = "";
        if(sentences.Count == 0)
        {
            endDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        textbox.text = sentence;
    }
    public void endDialogue()
    {
        nextButton.SetActive(false);
        Alexandra.SetActive(false);
        Debug.Log("End of intro");
    }
}
