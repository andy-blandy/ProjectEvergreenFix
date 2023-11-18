using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMission", menuName = "Mission", order = 1)]
public class Mission : ScriptableObject
{
    // Tasks
    public List<TaskObject> taskObjects;
    public List<Task> m_tasks;
    public enum TaskType { Building, Stat };

    [Header("UI")]
    public string title;
    public Sprite missionIcon;
    public string rewardDescription;
    [HideInInspector] public MissionCard card;

    // Dialogue
    public Dialogue introDialogue;
    public Dialogue completeDialogue;

    public void AddTasks()
    {
        m_tasks = new List<Task>();

        foreach (TaskObject taskObj in taskObjects)
        {
            switch (taskObj.type)
            {
                case TaskType.Building:
                    BuildingTask buildTask = card.gameObject.AddComponent<BuildingTask>();
                    buildTask.Copy(taskObj);
                    m_tasks.Add(buildTask);
                    break;
                case TaskType.Stat:
                    StatTask statTask = card.gameObject.AddComponent<StatTask>();
                    statTask.Copy(taskObj);
                    m_tasks.Add(statTask);
                    break;
                default:
                    break;
            }
        }

        card.SetTasks();
    }

    public void OnTaskComplete(Task task)
    {
        card.UpdateTask(task);
        CheckForMissionComplete();
    }

    public void OnTaskIncomplete(Task task)
    {
        card.UpdateTask(task);
    }

    private void CheckForMissionComplete()
    {
        foreach (Task task in m_tasks)
        {
            if (!task.isTaskComplete)
            {
                return;
            }
        }

        MissionCompleted();
    }

    // Override this to provide a reward?
    public virtual void MissionCompleted()
    {
        // TO-DO: Give the player a reward

        // TO-DO: Activate another dialogue

        // TO-DO: Play mission complete sound

        // TO-DO: Play mission complete animation

        // TO-DO: Remove mission card
    }
}
