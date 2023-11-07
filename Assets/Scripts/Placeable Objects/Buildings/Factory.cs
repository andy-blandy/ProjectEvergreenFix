using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : Building
{
    public int jobs = 100;
    public int impact = 20;
    public int powerGenerated = 1000;

    public override void Placed()
    {
        if (isPowered == true)
        {
            GameManager.instance.addJobs(jobs);
            GameManager.instance.addImpact(impact);
            GameManager.instance.addPower(powerGenerated);
        }
    }
}
