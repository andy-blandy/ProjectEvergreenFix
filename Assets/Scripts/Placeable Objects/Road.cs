using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Road : PlaceableObject
{
    [Header("Road Connections")]
    [Tooltip("0 = right, 1 = left, 2 = forward, 3 = back")]
    public List<Road> connectedRoads = new List<Road>(4) { null, null, null, null };
    public List<Building> connectedBuildings = new List<Building>(4) { null, null, null, null };

    public Tile connectedTile;

    public Vector3 positionOfRoad;

    void Start() 
    {
        connectedRoads = new List<Road>(4) { null, null, null, null };
        connectedBuildings = new List<Building>(4) { null, null, null, null };
    }

    public override void OnPlace()
    {
        CheckForConnections();
    }

    public void CheckForConnections()
    {
        // Check all four directions
        CheckTileForObject(transform.position + Vector3.right, 0);
        CheckTileForObject(transform.position + Vector3.left, 1);
        CheckTileForObject(transform.position + Vector3.forward, 2);
        CheckTileForObject(transform.position + Vector3.back, 3);
    }

    public void CheckTileForObject(Vector3 tilePosition, int index)
    {
        if (!TileManager.instance.tileMap.ContainsKey(tilePosition)) 
        {
            Debug.Log("TILEMAP HAS NO TILE AT " + tilePosition);
            return;
        }

        PlaceableObject obj = TileManager.instance.tileMap[tilePosition].heldObject;

        if (obj == null)
        {
            connectedRoads[index] = null;
            connectedBuildings[index] = null;
            return;
        }

        if (obj.TryGetComponent<Building>(out Building building))
        {
            connectedBuildings[index] = building;
        }

        if (obj.TryGetComponent<Road>(out Road road))
        {
            connectedRoads[index] = road;
        }
    }

    public House SearchForResidential()
    {
        Debug.Log("BEGINNING SEARCH");

        List<Road> visitedRoads = new List<Road>();
        Queue<Road> roadQueue = new Queue<Road>();

        CheckForConnections();

        foreach (Building building in connectedBuildings)
        {
            if (building == null)
            {
                continue;
            }

            if (building.type == Building.BuildingType.House)
            {
                if (!building.GetComponent<House>().AtCapacity())
                {
                    return building.GetComponent<House>();
                }
            }
        }

        visitedRoads.Add(this);
        foreach (Road road in connectedRoads)
        {
            if (road == null)
            {
                continue;
            }

            roadQueue.Enqueue(road);
        }

        while (roadQueue.Count > 0)
        {
            Road currentRoad = roadQueue.Dequeue();
            visitedRoads.Add(currentRoad);

            currentRoad.CheckForConnections();

            foreach (Building nextBuilding in currentRoad.connectedBuildings)
            {
                if (nextBuilding == null)
                {
                    continue;
                }

                if (nextBuilding.type == Building.BuildingType.House)
                {
                    if (!nextBuilding.GetComponent<House>().AtCapacity())
                    {
                        return nextBuilding.GetComponent<House>();
                    }
                }
            }

            foreach (Road nextRoad in currentRoad.connectedRoads)
            {
                if (nextRoad == null)
                {
                    continue;
                }

                if (visitedRoads.Contains(nextRoad))
                {
                    continue;
                }

                roadQueue.Enqueue(nextRoad);
            }
        }

        return null;
    }
}