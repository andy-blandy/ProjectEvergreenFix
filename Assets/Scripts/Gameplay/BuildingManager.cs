/*
 * Written by Andrew
 * 
 * Description:
 * This class will store and manage all of the placed buildings in the game.
 * This will include functions to collect resources from each building, such as income or power.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private List<Building> placedBuildings;

    public static BuildingManager instance;

    void Awake()
    {
        placedBuildings = new List<Building>();

        // Singleton
        instance = this;
    }

    /// <summary>
    /// Add a building to the list of placed buildings
    /// </summary>
    /// <param name="buildingToAdd"></param>
    public void AddBuilding(Building buildingToAdd)
    {
        placedBuildings.Add(buildingToAdd);
    }

    /// <summary>
    /// Iterate through a list of each building and get the total income from each.
    /// </summary>
    /// <returns>The combined income of every building that's been placed</returns>
    public int CollectBuildingIncome()
    {
        int totalIncome = 0;

        foreach (Building building in placedBuildings)
        {
            /*
             * Insert code here to add the income of each building to the totalIncome
             * Dependent on the Building base class that Adele will be making
             */
        }

        return totalIncome;
    }
}
