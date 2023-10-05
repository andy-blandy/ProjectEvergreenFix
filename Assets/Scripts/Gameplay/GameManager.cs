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
}
