//Written by Adele Rousseau
// @oracle1812 on discord
//Description: This sets the stadium script, it inherits from the building class and overwrites some of the methods.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stadiumScript : Building
{
    public int happiness = 500;
    public int jobs = 200;
    public int impact = 40;

    void Start()
    {
        type = BuildingType.Stadium;
    }
    public override void Placed()
    {
        if (isPowered == true)
        {
            GameManager.instance.addHappiness(happiness);
            GameManager.instance.addJobs(jobs);
            GameManager.instance.addImpact(impact);
        }
    }
    public override void Removed()
    {
        GameManager.instance.subtractHappiness(happiness);
        GameManager.instance.subtractJobs(jobs);
        GameManager.instance.subtractImpact(impact);
    }
}
