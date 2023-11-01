using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : Building
{
    // These variables should be public or serialized so that we can view and edit them in the Inspector
    bool isPowered = true;
    int jobs = 100;
    int impact = 20;
    int powerGenerated = 1000;

    void Start()
    {
        // These two lines should be removed since the Placed method has the same function
        GameManager.instance.addJobs(jobs);
        GameManager.instance.addImpact(impact);
    }

    // The Update method should be removed from the code if you aren't going to be using it, it can affect performance
    void Update()
    {
        
    }

    public override void placed()
    {
        if (isPowered == true)
        {
            GameManager.instance.addJobs(jobs);
            GameManager.instance.addImpact(impact);
            GameManager.instance.addPower(powerGenerated);
        }
    }
}
