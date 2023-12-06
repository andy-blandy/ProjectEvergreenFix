//Written by Adele Rousseau
// @oracle1812 on discord
//Description: This sets the house script, it inherits from the building class and overwrites some of the methods.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : Building
{
    // These variables should be public or serialized so that we can view and edit them in the Inspector
    public int popIncrease = 4;
    //int powerCost = 10;

    void Start()
    {
        type = BuildingType.House;
    }

    public override void Placed()
    {
        if (isPowered == true)
        {
            GameManager.instance.addPopulation(popIncrease);
        }
    }
    public override void Removed()
    {
        GameManager.instance.subtractPopulation(popIncrease);
    }
}
