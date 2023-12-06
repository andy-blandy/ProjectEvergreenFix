//Written by Adele Rousseau
// @oracle1812 on discord
//Description: This sets the office building script, it inherits from the building class and overwrites some of the methods.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeBuilding : Building
{
    public int happiness = 10;
    public int jobs = 50;
    public int impact = 5;

    public override void Placed()
    {
        if (isPowered == true)
        {
            GameManager.instance.addJobs(jobs);
            GameManager.instance.addImpact(impact);
            GameManager.instance.addHappiness(happiness);
        }
    }
    public override void Removed()
    {
        GameManager.instance.subtractJobs(jobs);
        GameManager.instance.subtractImpact(impact);
        GameManager.instance.subtractHappiness(happiness);
    }
}
