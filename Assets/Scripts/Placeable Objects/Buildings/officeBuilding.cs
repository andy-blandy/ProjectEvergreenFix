using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeBuilding : Building
{
    public int happiness = 10;
    public int jobs = 50;
    public int impact = 5;

    void Awake()
    {
        type = BuildingType.OfficeBuilding;
    }

    public override void Placed()
    {
        if (isPowered == true)
        {
            GameManager.instance.addJobs(jobs);
            GameManager.instance.addImpact(impact);
            GameManager.instance.addHappiness(happiness);
        }
    }
}
