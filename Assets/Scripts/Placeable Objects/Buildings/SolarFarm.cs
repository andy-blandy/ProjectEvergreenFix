using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarFarm : Building
{
    public int power = 10;

    void Awake()
    {
        type = BuildingType.SolarFarm;
    }

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
