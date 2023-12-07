//Written by Adele Rousseau
// @oracle1812 on discord
//Description: This sets the house script, it inherits from the building class and overwrites some of the methods.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : Building
{
    // These variables should be public or serialized so that we can view and edit them in the Inspector
    public int heldPop = 0;
    public int popCapacity = 4;
    //int powerCost = 10;

    void Start()
    {
        type = BuildingType.House;
    }

    public override void Placed()
    {
        heldPop = 0;
    }
    public override void Removed()
    {
    }

    public bool AtCapacity()
    {
        if (heldPop < popCapacity)
        {
            return false;
        } else
        {
            return true;
        }
    }
}
