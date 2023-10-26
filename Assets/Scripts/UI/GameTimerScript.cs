using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimerScript : MonoBehaviour
{
    //This is the text that pops up on the screen
    [Header("Month and Year")]
    public TextMeshProUGUI MonthTXT;
    public TextMeshProUGUI YearTXT;

    //This is the list of the months in the year as well as the year in an intager
    private List<string> months = new List<string>();
    private int year;

    //This is the total amount of time that the game has been running
    private float GameTimeTotal;

    //This is the time in the game that the month last updated
    private float LastUpdate;

    //This is a place holder variable to tell which month in the year the play is in
    private int MonthPlace = 0;

    //This timer is how long a month will last in game time
    [Header("Month Timer (In seconds)")]
    public int monthTimer;

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

        monthTimer = 120;

        MonthTXT.text = months[MonthPlace];

        year = 2023;
        YearTXT.text = year.ToString();
    }

    public void Update()
    {

        //This checks the if the Total game time minus the last update is the "month timer"
        //Then it changes the month that is displayed on the screen
        if (GameTimeTotal - LastUpdate >= monthTimer)
        {
            LastUpdate = GameTimeTotal;

            if (MonthPlace == 11)
            {
                MonthPlace = 0;
                year += 1;
                YearTXT.text = year.ToString();
            }
            else
            {
                MonthPlace++;
            }

            MonthTXT.text = months[MonthPlace];
        }

        GameTimeTotal += Time.deltaTime;
    }
}
