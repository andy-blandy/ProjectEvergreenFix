using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : Building
{
    // These variables should be public or serialized so that we can view and edit them in the Inspector
    public int popIncrease = 4;
    //int powerCost = 10;

    public override void Placed()
    {
        if (isPowered == true)
        {
            GameManager.instance.addPopulation(popIncrease);
        }
    }
}
