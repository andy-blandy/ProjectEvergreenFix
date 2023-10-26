/*
 * Written by Andrew Bland
 * @andy_blandy on Discord
 * 
 * Description:
 * This script should control the flow of the entire game.
 * There's no use for it yet other than controlling the resources of the town
 * 
 * TO-DO:
 * - Add getter and setters for each stat
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera gameCamera;

    [Header("Static Resources")]
    public int money = 1000;
    public int happiness;
    public int citizens;
    public int envImpact;
    public int power;

    [Header("Dynamic Resources")]
    public int jobs;
    public int populationCapacity;


    // Singleton
    [HideInInspector] public static GameManager instance;
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
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
        return citizens;
    }
    public int getEnvImpact()
    {
        return envImpact;
    }
    public int getPower()
    {
        return power;
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
        citizens += newCitizens;
    }
    public void subtractCitizens(int loss)
    {
        citizens -= loss;
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
        power += newPower;
    }
    public void subtractPower(int loss)
    {
        power -= loss;
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
}
