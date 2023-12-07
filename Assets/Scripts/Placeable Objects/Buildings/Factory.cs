//Written by Adele Rousseau
// @oracle1812 on discord
//Description: This sets the factory script, it inherits from the building class and overwrites some of the methods.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : Building
{
    public int jobs = 100;
    public int impact = 20;
    public int powerGenerated = 1000;

    void Start()
    {
        type = BuildingType.Factory;
    }
    public override void Placed()
    {
        if (isPowered == true)
        {
            GameManager.instance.addJobs(jobs);
            GameManager.instance.addImpact(impact);
            GameManager.instance.addPower(powerGenerated);
        }
    }
    public override void Removed()
    {
        GameManager.instance.subtractJobs(jobs);
        GameManager.instance.subtractImpact(impact);
        GameManager.instance.subtractPower(powerGenerated);
    }
}
