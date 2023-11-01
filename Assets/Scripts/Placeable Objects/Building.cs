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
using System.Linq;
using UnityEngine;

public class Building : PlaceableObject
{
    [Header("States")]
    public bool isConnectedToRoads;

    [Header("Resources")]
    public Dictionary<string, bool> resourceRequirementsMet;

    /// <summary>
    /// Each building will produce a resource for the player. This function will return the appropriate resource.
    /// </summary>
    /// <returns></returns>
    public virtual int GetResource()
    {
        Debug.Log("The resource for " + this.name + " has not been set!");
        int resourceValue = 0;
        return resourceValue;
    }

    // Method names should be capitalized
    public virtual void placed() { }
}
