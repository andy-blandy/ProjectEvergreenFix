using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    public string description;
    public bool isTaskComplete;
    public bool isActive;
    public int orderInMission;
    public Mission parentMission;

    public void CompleteTask()
    {
        isTaskComplete = true;
        parentMission.OnTaskComplete(this);
    }

    public void IncompleteTask()
    {
        isTaskComplete = false;
        parentMission.OnTaskIncomplete(this);
    }
}
