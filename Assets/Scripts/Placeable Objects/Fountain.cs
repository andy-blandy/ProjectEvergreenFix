using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fountain : Building
{
    int environmentalImpact = 1;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.subtractImpact(environmentalImpact);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
