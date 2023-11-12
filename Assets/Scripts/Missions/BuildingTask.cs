using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTask : Task
{

    public Building.BuildingType requiredBuilding;
    public int numOfBuildingsNeeded;
    public int buildingCount;

    void Awake()
    {
        buildingCount = 0;
        OnEnable();
    }

    void OnEnable()
    {
        BuildingManager.OnBuildingPlaced += CheckBuilding;
    }

    void OnDisable()
    {
        BuildingManager.OnBuildingPlaced -= CheckBuilding;
    }

    void CheckBuilding(Building building)
    {
        if (building.type == requiredBuilding)
        {
            buildingCount++;

            if (buildingCount >= numOfBuildingsNeeded)
            {
                CompleteTask();
            }
        }
    }
}
