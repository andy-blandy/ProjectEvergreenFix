using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : Building
{
    bool isPowered = true;
    int jobs = 100;
    int impact = 20;
    int powerGenerated = 1000;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.addJobs(jobs);
        GameManager.instance.addImpact(impact);
    }

    // Update is called once per frame
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
