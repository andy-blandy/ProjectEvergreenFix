using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public PlaceableObject heldObject;

    public Vector3 GetCenterOfObject()
    {
        Vector3 centerPos = transform.position;
        return centerPos;
    }
}
