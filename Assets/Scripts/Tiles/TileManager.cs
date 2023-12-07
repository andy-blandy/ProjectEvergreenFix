using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public Dictionary<Vector3, Tile> tileMap;

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
}
