//Written by Adele Rousseau
// @oracle1812 on discord
//Description: This sets the library script, it inherits from the building class and overwrites some of the methods.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Library : Building
{
    public int happiness = 10;
    public int jobs = 20;

    void Start()
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
    public override void Removed()
    {
        GameManager.instance.subtractHappiness(happiness);
        GameManager.instance.subtractJobs(jobs);
    }
}
