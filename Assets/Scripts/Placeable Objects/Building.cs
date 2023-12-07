/*
 * Written by Andrew Bland
 * @andy_blandy on Discord
 * 
 * Description:
 * This is the base class for all buildings in the game.
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Building : PlaceableObject
{
    [Header("States")]
    public bool isConnectedToRoads;
    public bool isPowered = true;
    public BuildingType type;

    [Header("Resources")]
    public Dictionary<string, int> resources;

    [Header("Stats")]
    public int powerCost = 1;

    public enum BuildingType 
    {
        Factory, 
        Fountain,
        House,
        Library,
        OfficeBuilding,
        SolarFarm,
        Windmill,
        Farm,
        Mall,
        MovieTheater,
        NuclearPlant,
        Park,
        Stadium
    }

    /// <summary>
    /// Each building will produce a resource for the player. This function will return the appropriate resource.
    /// </summary>
    /// <returns></returns>
    public virtual int GetResource(string resourceName)
    {
        int resourceValue = 0;

        if (!resources.ContainsKey(resourceName))
        {
            Debug.Log(this.name + " does not contain the resource " + resourceName + "!");
            return resourceValue;
        }

        resourceValue = resources[resourceName];
        return resourceValue;
    }

    // Method names should be capitalized
    public override void OnPlace()
    {
        Placed();
    }

    public virtual void Placed() 
    { 
    }

    public virtual void Removed()
    {

    }
}
