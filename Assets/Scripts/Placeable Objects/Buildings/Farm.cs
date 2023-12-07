//Written by Adele Rousseau
// @oracle1812 on discord
//Description: This sets the farm script, it inherits from the building class and overwrites some of the methods.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : Building
{
    public int jobs = 10;
    public int impact = 20;

    void Start()
    {
        type = BuildingType.Farm;
    }
    public override void Placed()
    {
        if (isPowered == true)
        {
            GameManager.instance.addJobs(jobs);
            GameManager.instance.subtractImpact(impact);
        }
    }
    public override void Removed()
    {
        GameManager.instance.subtractJobs(jobs);
        GameManager.instance.addImpact(impact);
    }
}
