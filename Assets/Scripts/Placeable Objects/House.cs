using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : Building
{
    bool isPowered = true;
    int popIncrease = 4;
    int powerCost = 10;
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
            GameManager.instance.addPopulation(popIncrease);
        }
    }
}