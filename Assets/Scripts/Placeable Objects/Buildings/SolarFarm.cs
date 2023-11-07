using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarFarm : Building
{
    public int power = 10;

    public override void Placed()
    {
        Debug.Log("SOLAR PLACED");
        GameManager.instance.addPower(power);
    }

    public override void Removed()
    {
        GameManager.instance.subtractPower(power);
    }
}
