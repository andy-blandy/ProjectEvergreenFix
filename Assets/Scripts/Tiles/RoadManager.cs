/*
 * Written by Andrew
 * @andy_blandy on Discord
 * 
 * Description:
 * This script will keep track of every road that's been placed
 * If the roads connect from the town center to a building, that building will be supplied power and can be travelled to
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public List<Road> roads;

    public static RoadManager instance;
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void AddRoad(Road inputRoad)
    {
        roads.Add(inputRoad);
    }

    void UpdateRoadTree()
    {

    }
}
