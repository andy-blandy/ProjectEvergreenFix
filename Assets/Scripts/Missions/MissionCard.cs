/*
 * Responsible for connecting the mission class to the UI object
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionCard : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public List<Toggle> taskToggles;
    public List<TextMeshProUGUI> taskText;
    public TextMeshProUGUI rewardText;
    public Image missionIconImage;

    public Mission connectedMission;

    void Start()
    {
        SetCard(connectedMission);
    }

    public void SetCard(Mission mission)
    {
        connectedMission = mission;

        titleText.text = mission.title;
        rewardText.text = mission.rewardDescription;

        SetTasks();
    }

    public void SetTasks()
    {
        //taskToggles = new List<Toggle>();
        //taskText = new List<TextMeshProUGUI>();

        for (int i = 0; i < connectedMission.tasks.Length; i++)
        {
            connectedMission.tasks[i].orderInMission = i;
            UpdateTask(connectedMission.tasks[i]);
        }
    }

    public void UpdateTask(Task task)
    {
        taskText[task.orderInMission].text = task.description;
        taskToggles[task.orderInMission].isOn = task.isTaskComplete;
    }
}
