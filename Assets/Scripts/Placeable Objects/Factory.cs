using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : Building
{
    bool isPowered = false;
    int jobs = 100;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.addJobs(jobs);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
