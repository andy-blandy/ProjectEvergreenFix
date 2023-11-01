using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Library : Building
{
    // These variables should be public or serialized so that we can view and edit them in the Inspector
    bool isPowered = true;
    int happiness = 10;
    int jobs = 20;

    // The Start and Update methods can be removed from the code if you aren't going to be using them, they can affect performance

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
            GameManager.instance.addHappiness(happiness);
            GameManager.instance.addJobs(jobs);
        }
    }
}
