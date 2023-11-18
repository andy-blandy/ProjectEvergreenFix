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

    public void SetCard(Mission mission)
    {
        // Logic
        connectedMission = mission;
        mission.card = this;

        // UI
        titleText.text = mission.title;
        rewardText.text = mission.rewardDescription;
        missionIconImage.sprite = mission.missionIcon;
    }

    public void SetTasks()
    {
        //taskToggles = new List<Toggle>();
        //taskText = new List<TextMeshProUGUI>();

        for (int i = 0; i < connectedMission.m_tasks.Count; i++)
        {
            connectedMission.m_tasks[i].orderInMission = i;
            UpdateTask(connectedMission.m_tasks[i]);
        }
    }

    public void UpdateTask(Task task)
    {
        Debug.Log(task.orderInMission);
        taskText[task.orderInMission].text = task.description;
        taskToggles[task.orderInMission].isOn = task.isTaskComplete;
    }
}
