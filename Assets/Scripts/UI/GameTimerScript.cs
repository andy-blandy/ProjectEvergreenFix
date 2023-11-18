using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimerScript : MonoBehaviour
{
    //This is the text that pops up on the screen
    [Header("Month and Year")]
    public TextMeshProUGUI monthTXT;
    public TextMeshProUGUI yearTXT;

    //This is the list of the months in the year as well as the year in an integer
    private List<string> months = new List<string>();
    public int currentYear = 2023;

    // This is the total amount of time that the game has been running
    public float gameTimeTotal;

    // This is a place holder variable to tell which month in the year the player is in
    public int monthPlace = 0;

    // This timer is how long a month will last in game time
    [Header("Month Timer (in seconds)")]
    public int monthTimer = 120;
    public float monthCountdown;

    public static GameTimerScript instance;
    void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Initializing the variables in the previous sections
    /// </summary>
    public void Start()
    {
        months.Add("Jan");
        months.Add("Feb");
        months.Add("Mar");
        months.Add("Apr");
        months.Add("May");
        months.Add("June");
        months.Add("July");
        months.Add("Aug");
        months.Add("Sep");
        months.Add("Oct");
        months.Add("Nov");
        months.Add("Dec");

        // Set GUI
        monthTXT.text = months[monthPlace];
        yearTXT.text = currentYear.ToString();

        // Set timers
        monthCountdown = monthTimer;
    }

    public void Update()
    {

        //This checks the if the Total game time minus the last update is the "month timer"
        //Then it changes the month that is displayed on the screen
        if (monthCountdown <= 0)
        {
            UpdateMonth();

            monthCountdown = monthTimer;
        }

        gameTimeTotal += Time.deltaTime;
        monthCountdown -= Time.deltaTime;
    }

    private void UpdateMonth()
    {
        if (monthPlace == 11)
        {
            monthPlace = 0;
            currentYear += 1;
            yearTXT.text = currentYear.ToString();
        }
        else
        {
            monthPlace++;
        }

        monthTXT.text = months[monthPlace];
    }
}
