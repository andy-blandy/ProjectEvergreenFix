using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{ 
    //The characters that can pop up on the screen
    [Header("The Tutors")]
    public GameObject Alex;
    public GameObject Cassius;
    public GameObject Lily;

    //The text and dialogue boxes that are on the screen
    [Header("DialogueBox")]
    public GameObject DialogueBackground;
    public TextMeshProUGUI DiaBox;

    //The buttons to click next and finish
    [Header("Buttons")]
    public GameObject NextButton;
    public GameObject FinishButton;

    //A list of sentences that the chatacters can say as 
    //well as the place in the list that the characters are
    private List<string> AlexSentences;
    private List<string> CassiusSentences;
    private List<string> LilySentences;
    private int AlexSentencePlace;
    private int CassiusSentencePlace;
    private int LilySentencePlace;

    private void Start()
    {
        //Adding sentences to the list of sentences that alex can say to the player
        AlexSentences = new List<string>();
        AlexSentences.Add("Hello my name is Alexandra and welcome to Project Evergreen!");
        AlexSentences.Add("In this tutorial level we will show you how to play.");
        AlexSentences.Add("To start this level, navigate to Edit menu in the bottom left."); //2
        AlexSentences.Add("Here you can see the options in the Edit menu, Destroy and Place.");
        AlexSentences.Add("Click Destroy to remove objects from the scene, then click the X in the top right when you are done."); //4
        AlexSentences.Add("As you can see, when you destory the enviornment your EI score increases. To win the game, you will want to keep this score as low as possible.");
        AlexSentences.Add("Next, click the Place Button."); //6
        AlexSentences.Add("The Place Menu will give you options on what building can be placed in your new City");
        AlexSentences.Add("Try clicking the Building button and placing a building around in your town. Buildings cost money so you have to watch how money you have available"); //8
        AlexSentences.Add("Click the X in the top right when you are done"); // 9
        AlexSentences.Add("Congratulations! You have built your first building!");
        AlexSentences.Add("Next, navigate to the Missions Menu in the bottom right"); //11
        AlexSentences.Add("In this menu, you are given missions that can completed. These missions can affect your enviornement as well as the money you make");
        AlexSentences.Add("Missions will appear here throughout your time playing so dont forget to check back here from time to time.");
        AlexSentences.Add("Finally, click the Close button to exit the Missions Menu."); //14
        AlexSentences.Add("CONGRATULATIONS!!");
        AlexSentences.Add("You have completed the tutorial and are ready to play the real thing and fight back against the Beavers!"); //16

        BeginAlexDialogue();
    }

    //Loops through and displays the sentence that alex is saying to the player at the time
    public void AlexNextSentence()
    {
        if((AlexSentencePlace + 1) < AlexSentences.Count)
        {
            AlexSentencePlace += 1;
            if (AlexSentencePlace == 2 || 
                AlexSentencePlace == 4 || 
                AlexSentencePlace == 6 || 
                AlexSentencePlace == 9 ||
                AlexSentencePlace == 11||
                AlexSentencePlace == 14||
                AlexSentencePlace == 16)
            {
                ShowFinishHideNext();
            }
        }
        DiaBox.text = AlexSentences[AlexSentencePlace];
    }

    //Shows the finish button and hides the next button from the player
    public void ShowFinishHideNext()
    {
        NextButton.SetActive(false);
        FinishButton.SetActive(true);
    }

    //Starts alex dialogue by showing the character and the dialogue box
    public void BeginAlexDialogue()
    {
        if (AlexSentencePlace < 16)
        {
            Alex.SetActive(true);
            DialogueBackground.SetActive(true);
            DiaBox.text = AlexSentences[AlexSentencePlace];
            FinishButton.SetActive(false);
            NextButton.SetActive(true);
        }
    }

    //Hides alex and her dialogue box as well as incriments where she is in her dialogue by 1
    public void FinishTextDiaLogue()
    {
        Alex.SetActive(false);
        DialogueBackground.SetActive(false);

        if ((AlexSentencePlace + 1) < AlexSentences.Count)
        {
            AlexSentencePlace += 1;
        }
    }
}

