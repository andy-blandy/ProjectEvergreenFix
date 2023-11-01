using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fountain : Building
{
    // This variable should be public or serialized so that we can view and edit them in the Inspector
    int environmentalImpact = 1;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.subtractImpact(environmentalImpact);
    }

    // The Update method should be removed from the code if you aren't going to be using it, it can affect performance
    void Update()
    {
        
    }
}
