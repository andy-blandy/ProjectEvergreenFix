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
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera gameCamera;

    public enum Stats
    {
        Money,
        Happiness,
        Population,
        Environment,
        AvailablePower,
        Jobs
    }

    [Header("Town Stats")]
    public int money = 1000;
    public int happiness;
    public int envImpact;
    public int availablePower;
    public int neededPower;
    public int jobs;
    public int population;
    public int populationCapacity;
    public Dictionary<Stats, int> townStats;

    [Header("Town States")]
    public bool inBlackout;


    // Singleton
    [HideInInspector] public static GameManager instance;
    void Awake()
    {
        instance = this;

        townStats = new Dictionary<Stats, int>();
    }

    void Start()
    {
        townStats.Add(Stats.Money, money);
        townStats.Add(Stats.Happiness, happiness);
        townStats.Add(Stats.Population, population);
        townStats.Add(Stats.Environment, envImpact);
        townStats.Add(Stats.AvailablePower, availablePower);
        townStats.Add(Stats.Jobs, jobs);

    }

    void Update()
    {
        CheckPower();
    }

    void BeginGame()
    {
        inBlackout = false;
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
}
