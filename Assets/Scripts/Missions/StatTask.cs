using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatTask : Task
{
    public GameManager.Stats watchedStat;
    public int reqLevelOfStat;

    void Update()
    {
        if (!isTaskComplete && 
            GameManager.instance.townStats[watchedStat] >= reqLevelOfStat)
        {
            CompleteTask();
        } else if (isTaskComplete &&
            GameManager.instance.townStats[watchedStat] < reqLevelOfStat)
        {
            IncompleteTask();
        }
    }
}
