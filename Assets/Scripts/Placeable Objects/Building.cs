/*
 * Written by Andrew Bland
 * @andy_blandy on Discord
 * 
 * Description:
 * This is a proof-of-concept prototype version of the building class for the tutorial level I'm making
 * This probably shouldn't be used in the final game without major QoL tweaks
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : PlaceableObject
{
    [Header("States")]
    public bool isConnectedToRoads;

}
