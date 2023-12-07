//Written by Adele Rousseau
// @oracle1812 on discord
//Description: This sets the fountain script, it inherits from the building class and overwrites some of the methods.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fountain : Building
{
    public int impact = 1;
    void Start()
    {
        type = BuildingType.Fountain;
    }
    public override void Placed()
    {
        if (isPowered == true)
        {
            GameManager.instance.subtractImpact(impact);

        }
    }
    public override void Removed()
    {
        GameManager.instance.addImpact(impact);

    }
}
