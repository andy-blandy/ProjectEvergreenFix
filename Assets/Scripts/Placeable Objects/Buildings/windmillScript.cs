//Written by Adele Rousseau
// @oracle1812 on discord
//Description: This sets the stadium script, it inherits from the building class and overwrites some of the methods.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windmillScript : Building
{
    public int jobs = 40;
    public int impact = 30;
    public int powerGenerated = 1500;

    void Start()
    {
        type = BuildingType.Windmill;
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
