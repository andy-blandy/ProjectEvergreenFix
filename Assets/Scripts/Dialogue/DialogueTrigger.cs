using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void StartDialogue()
    {
        DialogueManager.instance.StartDialogue(dialogue);
    }
    public Dialogue GetDialogue()
    {
        return dialogue;
    }
}
