using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionMenuPopup : MonoBehaviour
{
    public Transform missionContainer;
    public float spaceBetweenMissions = 10f;

    public static MissionMenuPopup instance;
    void Awake()
    {
        instance = this;
    }

    public void PositionCard(GameObject missionCard)
    {
        float yPosition = 15;

        foreach (Transform child in missionContainer)
        {
            if (child == missionCard.transform)
            {
                continue;
            }

            Debug.Log(child.name + " with height of " + child.GetComponent<RectTransform>().rect.height);
            yPosition += child.GetComponent<RectTransform>().rect.height;
            yPosition += spaceBetweenMissions;
        }

        yPosition *= -1;
        yPosition += 400;
        Vector3 newPosition = new Vector3(0, yPosition, 0);
        Debug.Log(newPosition.ToString());
        missionCard.GetComponent<RectTransform>().localPosition = newPosition;
    }
}
