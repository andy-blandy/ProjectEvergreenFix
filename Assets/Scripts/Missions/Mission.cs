using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour
{
    public string title;
    public Task[] tasks;
    public Sprite missionIcon;
    public string rewardDescription;

    public MissionCard card;

    public void OnTaskComplete(Task task)
    {
        card.UpdateTask(task);
        CheckForMissionComplete();
    }

    public void OnTaskIncomplete(Task task)
    {
        card.UpdateTask(task);
    }

    private void CheckForMissionComplete()
    {
        foreach (Task task in tasks)
        {
            if (!task.isTaskComplete)
            {
                return;
            }
        }

        MissionCompleted();
    }

    // Override this to provide a reward?
    public virtual void MissionCompleted()
    {
        // TO-DO: Give the player a reward

        // TO-DO: Activate another dialogue

        // TO-DO: Play mission complete sound

        // TO-DO: Play mission complete animation
    }
}
