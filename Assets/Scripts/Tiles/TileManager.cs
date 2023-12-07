using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public Dictionary<Vector3, Tile> tileMap;


    [Header("Tile Types")]
    public GameObject grassTilePrefab;

    public static TileManager instance;
    private void Awake()
    {
        instance = this;

        tileMap = new Dictionary<Vector3, Tile>();

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            if (child.TryGetComponent<Tile>(out Tile newTile))
            {
                if (tileMap.ContainsKey(child.position))
                {
                    Destroy(child.gameObject);
                    continue;
                }

                tileMap.Add(child.position, newTile);
            }
        }
    }

    public void GenerateBox(Vector3 center, Vector3 size)
    {
        for (int x = 0; x <= size.x; x++)
        {
            for (int z = 0; z < size.z; z++)
            {
                Vector3 spawnPos = new Vector3(center.x + x + (size.x * -0.5f), 0, center.z + z + (size.z * -0.5f));
                SpawnTile(spawnPos);
            }
        }
    }

    public void SpawnTile(Vector3 position)
    {
        GameObject newTile = Instantiate(grassTilePrefab, position, Quaternion.Euler(90f, 0, 0), transform);
        tileMap.Add(position, newTile.GetComponent<Tile>());
    }
}
