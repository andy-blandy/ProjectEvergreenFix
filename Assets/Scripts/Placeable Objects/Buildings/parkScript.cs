//Written by Adele Rousseau
// @oracle1812 on discord
//Description: This sets the park script, it inherits from the building class and overwrites some of the methods.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parkScript : Building
{
    public int happiness = 10;
    public int impact = 10;
    void Start()
    {
        type = BuildingType.Park;
    }
    public override void Placed()
    {
            GameManager.instance.addHappiness(happiness);
            GameManager.instance.addImpact(impact);
    }
    public override void Removed()
    {
        GameManager.instance.subtractHappiness(happiness);
        GameManager.instance.subtractImpact(impact);
    }
}
