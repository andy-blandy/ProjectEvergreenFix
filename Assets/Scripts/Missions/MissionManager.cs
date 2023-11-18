using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [Header("Missions")]
    public List<Mission> allMissions;
    public Mission queuedMission;

    [Header("UI")]
    public GameObject missionCardPrefab;
    public MissionMenuPopup missionMenuPopup;

    void Start()
    {
        missionMenuPopup = MissionMenuPopup.instance;
    }

    void Update()
    {
        // Start mission at a specific time
        // This if-statement is terrible for many reasons; mostly because it will be called hundreds of times
        // This needs to be burned with fire and rebuilt
        if (GameTimerScript.instance.gameTimeTotal >= 5 && GameTimerScript.instance.gameTimeTotal <= 6)
        {
            BeginMission(allMissions[0]);
        }
    }

    public void BeginMission(Mission mission)
    {
        if (queuedMission != null)
        {
            Debug.Log("Error! Trying to begin too many missions at once.");
            return;
        }

        // Begin mission dialogue
        queuedMission = mission;
        DialogueManager.instance.StartDialogue(mission.introDialogue);

        // Subscribe to EndDialogue event to add the mission once the dialogue is completed
        DialogueManager.OnDialogueEnded += AddMission;
    }

    public void AddMission()
    {
        // Unsubscribe from EndDialogue event
        DialogueManager.OnDialogueEnded -= AddMission;

        // TO-DO - VFX of adding mission

        // TO-DO - Sound FX of adding mission

        // TO-DO - Add mission to mission menu
        GameObject missionCard = Instantiate(missionCardPrefab, missionMenuPopup.missionContainer);
        missionCard.GetComponent<MissionCard>().SetCard(queuedMission);
        missionMenuPopup.PositionCard(missionCard);

        // Initalize needed variables for mission
        queuedMission.AddTasks();

        // Reset queued mission
        queuedMission = null;
    }
}
