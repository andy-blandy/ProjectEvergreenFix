using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    // Remove the Start and Update functions if you aren't going to use them; this can affect performance in the future
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDialogue()
    {
        // Try to avoid using 'FindObjectOfType' when possible, it's not great for performance.
        // You can add a singleton to the DialogueManager and use the instance of that; see lines 31 - 37 of GameManager for an example.
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
    public Dialogue GetDialogue()
    {
        return dialogue;
    }
}
