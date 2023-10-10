using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public Transform mapHolder;

    void Awake()
    {
        // If any children objects in the map holder do not have the tile script, give it to them
        for (int i = 0; i < mapHolder.childCount; i++)
        {
            GameObject curChild = mapHolder.GetChild(i).gameObject;
            if (curChild.GetComponent<Tile>() == null)
            {
                curChild.AddComponent<Tile>();
            }
        }
    }
}
