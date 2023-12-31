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
    [Header("Graphics")]
    public Material errorMaterial;
    public Material guideMaterial;

    [Header("Game Logic")]
    [SerializeField] private List<Building> placedBuildings;

    // Events
    public delegate void BuildingPlaced(Building placedBuilding);
    public static event BuildingPlaced OnBuildingPlaced;

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
        // Add to list of buildings
        placedBuildings.Add(buildingToAdd);

        if (OnBuildingPlaced != null)
        {
            OnBuildingPlaced(buildingToAdd);
        }

        // Power
        GameManager.instance.neededPower += buildingToAdd.powerCost;
        if (GameManager.instance.neededPower <= GameManager.instance.availablePower)
        {
            buildingToAdd.isPowered = true;
            buildingToAdd.Placed();
        }
    }

    public void PowerDownBuildings()
    {
        foreach (Building building in placedBuildings)
        {
            building.isPowered = false;
        }
    }

    public void PowerUpBuildings()
    {
        foreach (Building building in placedBuildings)
        {
            building.isPowered = true;
        }
    }

    /// <summary>
    /// Iterates through every buildng in the placedBuildings list and calculates the total of all the buildings' power cost. Then, the neededPower stat in the active game manager is set
    /// to this total.
    /// </summary>
    /// <returns></returns>
    public void UpdateNeededPower()
    {
        int totalNeededPower = 0;

        foreach (Building building in placedBuildings)
        {
            totalNeededPower += building.powerCost;
        }
        
        GameManager.instance.neededPower = totalNeededPower;
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
