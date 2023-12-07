using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : PlaceableObject
{
    [Header("Road Connections")]
    public List<Road> connectedRoads = new List<Road>(4);

    // This allows us to communicate with RoadManager whenever a road is placed
    public delegate void RoadPlacedAction();
    public event RoadPlacedAction OnRoadPlaced;

    public void CheckForConnectingRoads()
    {

    }

    public override void OnPlace()
    {
        //OnRoadPlaced();
    }
}