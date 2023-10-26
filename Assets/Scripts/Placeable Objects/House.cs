using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : Building
{
    bool powered = false;
    int popIncrease = 4;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.addPopulation(popIncrease);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
