using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : PlaceableObject
{
    [Header("Road Connections")]
    public Transform[] connectors;

    public bool isConnectedToTownSquare;

    // This allows us to communicate with RoadManager whenever a road is placed
    public delegate void RoadPlacedAction();
    public event RoadPlacedAction OnRoadPlaced;

    void Awake()
    {

    }

    void CheckForConnections()
    {
        bool[] isConnected = new bool[connectors.Length];
        int index = 0;
        foreach (Transform connector in connectors)
        {
            RaycastHit hit;

            if (Physics.Raycast(connector.position, connector.forward, out hit, 1.0f, 7))
            {
                if (hit.transform.GetComponent<Road>().isConnectedToTownSquare)
                {
                    isConnected[index] = true;
                }
            }

            index++;
        }

        for (int i = 0; i < isConnected.Length; i++)
        {
            if (!isConnected[i]) 
            { 
                isConnectedToTownSquare = false;
                return;
            }
        }

        isConnectedToTownSquare = true;
    }

    public override void OnPlace()
    {
        //OnRoadPlaced();
    }
}