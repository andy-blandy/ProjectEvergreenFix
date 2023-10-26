using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Library : Building
{
    bool isPowered = false;
    int happiness = 10;
    int jobs = 20;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.addHappiness(happiness);
        GameManager.instance.addJobs(jobs);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
