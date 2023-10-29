using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class officeBuilding : Building
{
    bool isPowered = true;
    int happiness = 10;
    int jobs = 50;
    int impact = 5;
    // Start is called before the first frame update
    void Start()
    {
        
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
            GameManager.instance.addHappiness(happiness);
        }
    }
}
