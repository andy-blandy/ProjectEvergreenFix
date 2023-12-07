/*
 * Written by Andrew Bland
 * @andy_blandy on Discord
 * 
 * Description:
 * This script should control the flow of the entire game.
 * There's no use for it yet other than controlling the resources of the town
 * 
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera gameCamera;

    public Road startingRoad;

    public string townName;

    [Header("Town Size")]
    public Vector3 mapSize;
    public Vector3 mapCenter;

    [Header("Town Stats")]
    public int money = 1000;
    public int happiness;
    public int envImpact;
    public int availablePower;
    public int neededPower;
    public int jobs;
    public int population;
    public int populationCapacity;
    public enum Stats
    {
        Money,
        Happiness,
        Population,
        Environment,
        AvailablePower,
        Jobs
    }
    public Dictionary<Stats, int> townStats;

    [Header("Town States")]
    public bool inBlackout;


    [Header("Timers")]
    public float timeBetweenPopIncrease = 10f;
    public float newPopTimer;

    // Singleton
    [HideInInspector] public static GameManager instance;
    void Awake()
    {
        instance = this;

        townStats = new Dictionary<Stats, int>
        {
            { Stats.Money, money },
            { Stats.Happiness, happiness },
            { Stats.Population, population },
            { Stats.Environment, envImpact },
            { Stats.AvailablePower, availablePower },
            { Stats.Jobs, jobs }
        };
    }

    void Start()
    {
        inBlackout = false;

    }

    void Update()
    {
        CheckPower();

        newPopTimer += Time.deltaTime;
        if (newPopTimer > timeBetweenPopIncrease)
        {
            AddPopulation();
            newPopTimer = 0;
        }
    }

    public void CheckPower()
    {
        if (availablePower < neededPower && !inBlackout)
        {
            inBlackout = true;
            BuildingManager.instance.PowerDownBuildings();
        }
        else if (availablePower >= neededPower && inBlackout)
        {
            inBlackout = false;
            BuildingManager.instance.PowerUpBuildings();
        }
    }

    public void AddPopulation()
    {
        House house = startingRoad.SearchForResidential();

        if (house == null)
        {
            Debug.Log("NO HOUSE FOUND");
        } else
        {
            house.heldPop += 1;
            population += 1;
        }
    }

    #region Getters and Setters
    public int getMoney()
    {
        return money;
    }
    public int getHappiness()
    {
        return happiness;
    }
    public int getCitizens()
    {
        return population;
    }
    public int getEnvImpact()
    {
        return envImpact;
    }
    public int getPower()
    {
        return availablePower;
    }
    public int getJobs()
    {
        return jobs;
    }
    public int getPopCapacity()
    {
        return populationCapacity;
    }

    public void SetTownName(string name)
    {
        townName = name;
        PlayerHUD.instance.UpdateTownName();
    }

    public void SetTownName(TMP_InputField inputField)
    {
        if (inputField.text == "")
        {
            return;
        }

        SetTownName(inputField.text);
    }

    public void addMoney(int newMoney)
    {
        money += newMoney;
    }
    public void subtractMoney(int loss)
    {
        money -= loss;
    }

    public void addHappiness(int newHappiness)
    {
        happiness += newHappiness;
    }
    public void subtractHappiness(int loss)
    {
        happiness -= loss;
    }

    public void addCitizens(int newCitizens)
    {
        population += newCitizens;
    }
    public void subtractCitizens(int loss)
    {
        population -= loss;
    }

    public void addImpact(int newImpact)
    {
        envImpact += newImpact;
    }
    public void subtractImpact(int newImpact)
    {
        envImpact -= newImpact;
    }

    public void addPower(int newPower)
    {
        availablePower += newPower;
    }
    public void subtractPower(int loss)
    {
        availablePower -= loss;
    }

    public void addJobs(int newJobs)
    {
        jobs += newJobs;
    }
    public void subtractJobs(int loss)
    {
        jobs -= loss;
    }

    public void addPopulation(int addPop)
    {
        populationCapacity += addPop;
    }
    public void subtractPopulation(int loss)
    {
        populationCapacity -= loss;
    }
    #endregion

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(mapCenter, mapSize);
    }
}
