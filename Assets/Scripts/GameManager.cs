using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Static Resources")]
    public int money;
    public int happiness;
    public int citizens;
    public int impact;

    [Header("Dynamic Resources")]
    public int jobs;
    public int populationCap;


    // Singleton
    [HideInInspector] public static GameManager instance;
    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
