using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Library : Building
{
    public int happiness = 10;
    public int jobs = 20;

    void Awake()
    {
        type = BuildingType.Library;
    }

    public override void Placed()
    {
        if (isPowered == true)
        {
            GameManager.instance.addHappiness(happiness);
            GameManager.instance.addJobs(jobs);
        }
    }
}
