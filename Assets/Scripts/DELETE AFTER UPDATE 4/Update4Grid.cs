/*
 * Written by Trevor
 * Edited by Andrew on 10/15
 * 
 * Description:
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Update4Grid : MonoBehaviour
{
    [Header("Map Settings")]
    public Vector2Int mapSize;
    public Tile[,] tiles;

    [Header("Prefabs")]
    public GameObject tilePrefab;

    [Header("Game References")]
    public GameObject mapHolder;
    public Camera gamecam;

    [Header("Scene View")]
    public bool randomlyGenerateMap = true;
    public bool showMapBounds = true;

    List<Building> gridObjects = new List<Building>();
    List<ResourceGrid> resourceGrids = new List<ResourceGrid>();

    void Start()
    {
        InitializeMap();
        GetTilesFromMapHolder();

        if (randomlyGenerateMap)
        {
            SpawnRemainingTiles();
        }
    }

    void Update()
    {
    }

    /// <summary>
    /// Initializes all the variables of the map (tiles, gridObjects)
    /// </summary>
    public void InitializeMap()
    {
        tiles = new Tile[mapSize.x, mapSize.y];

        gridObjects = new List<Building>();
    }


    /// <summary>
    /// Adds every gameobject that is a child of the mapHolder to the tiles array. If the gameObject does not have the Tile class attached, add it.
    /// </summary>
    /// <returns>False if any child of the mapHolder is outside of the map's bounds</returns>
    private bool GetTilesFromMapHolder()
    {
        List<Transform> checkedObjects = new List<Transform>();

        int row = 0;
        int col = 0;

        foreach (Transform t in mapHolder.transform)
        {
            // Make sure all tiles are within the map bounds
            if (t.position.x < 0 ||
                t.position.z < 0 ||
                t.position.x > (mapSize.x - 1) ||
                t.position.z > (mapSize.y - 1))
            {
                Debug.Log("Gameobject: " + t.name + " couldn't be added because it is out of level bounds, please make sure all prefab objects are placed within map bounds.");
                return false;
            }

            row = (int)t.position.x;
            col = (int)t.position.z;

            if (t.TryGetComponent<Tile>(out Tile tile))
            {
                tiles[row, col] = tile;
            }
            else
            {
                Tile newTile = t.gameObject.AddComponent<Tile>();
                tiles[row, col] = newTile;
            }
        }

        return true;
    }

    /// <summary>
    /// Generates the rest of the map according to the map size.
    /// </summary>
    void SpawnRemainingTiles()
    {
        for (int col = 0; col < mapSize.x; col++)
        {
            for (int row = 0; row < mapSize.y; row++)
            {
                // Do nothing if a tile already exists in the current spot
                if (tiles[row, col] != null)
                {
                    continue;
                }

                // Spawn a new tile prefab
                Tile newTile = Instantiate(tilePrefab, new Vector3(row, 0, col), Quaternion.identity, mapHolder.transform).GetComponent<Tile>();

                // Set the tile to a random type
                int randomInt = Random.Range(0, 1);
                if (randomInt == 0)
                {
                    newTile.tileType = "forest";
                }
                else if (randomInt == 1)
                {
                    newTile.tileType = "plains";
                }
            }
        }
    }

    /// <summary>
    /// Draws gizmos for the scene view. Currently only draws a green square in the scene showing the bounds of the map.
    /// </summary>
    public void OnDrawGizmos()
    {
        if (showMapBounds)
        {
            // Get the bounds of the map
            Vector3[] points = new Vector3[8]
            {
            new Vector3(0, 0, 0),
            new Vector3(mapSize.x, 0, 0),
            new Vector3(mapSize.x, 0, 0),
            new Vector3(mapSize.x, 0, mapSize.y),
            new Vector3(mapSize.x, 0, mapSize.y),
            new Vector3(0, 0, mapSize.y),
            new Vector3(0, 0, mapSize.y),
            new Vector3(0, 0, 0)
            };

            // Draw the bounds of the map when the grid manager is selected
            Gizmos.color = Color.green;
            Gizmos.DrawLineList(points);
        }
    }
}