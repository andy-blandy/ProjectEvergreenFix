using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Library : Building
{
    bool isPowered = true;
    int happiness = 10;
    int jobs = 20;
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
        if(isPowered == true)
        {
            GameManager.instance.addHappiness(happiness);
            GameManager.instance.addJobs(jobs);
        }
    }
}
