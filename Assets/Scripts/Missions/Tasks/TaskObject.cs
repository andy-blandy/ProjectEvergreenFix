using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TaskObject
{
    [Header("Task Properties")]
    public Mission.TaskType type;
    public string description;

    [Header("Building Task")]
    public Building.BuildingType reqBuilding;
    public int numOfBuildingsNeeded;

    [Header("Stat Task")]
    public GameManager.Stats watchedStat;
    public int reqLevelOfStat;
}
