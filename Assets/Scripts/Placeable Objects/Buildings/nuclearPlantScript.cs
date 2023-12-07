//Written by Adele Rousseau
// @oracle1812 on discord
//Description: This sets the nuclear plant script, it inherits from the building class and overwrites some of the methods.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nuclearPlantScript : Building
{
    public int jobs = 200;
    public int impact = 40;
    public int powerGenerated = 2000;

    void Start()
    {
        type = BuildingType.NuclearPlant;
    }
    public override void Placed()
    {
        if (isPowered == true)
        {
            GameManager.instance.addJobs(jobs);
            GameManager.instance.subtractImpact(impact);
            GameManager.instance.addPower(powerGenerated);
        }
    }
    public override void Removed()
    {
        GameManager.instance.subtractJobs(jobs);
        GameManager.instance.addImpact(impact);
        GameManager.instance.subtractPower(powerGenerated);
    }
}
