using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void startDialogue()
    {
        FindObjectOfType<DialogueManager>().startDialogue(dialogue);
    }
    public Dialogue getDialogue()
    {
        return dialogue;
    }
}
