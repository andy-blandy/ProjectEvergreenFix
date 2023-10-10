/*
 * Written by Andrew Bland
 * @andy_blandy on Discord
 * 
 * Description:
 * This will be attached to each button in the build menu of the Player HUD.
 * After attaching the objectPrefab to this script, the button will update it's UI appropriately
 * This will pass the correct object prefab to PlayerControls when clicked.
 * To set this up, attach to a button and connect all the components of the button in the inspector
 * Go to Prefabs > UI > BuildingButton for an example
 * Make sure to reference a prefab with the "PlaceableObject" class attached
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectIcon : MonoBehaviour
{
    public PlaceableObject objectPrefab;

    [Header("UI")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI costText;
    public Image icon;
    public GameObject hoverMenu;

    [Header("References")]
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;

        SetUI();
    }

    /// <summary>
    /// Sets all of the UI objects to the corresponding values from the connected buildingPrefab.
    /// </summary>
    public void SetUI()
    {
        // If there's no building attached to this button, hide it
        if (objectPrefab == null)
        {
            gameObject.SetActive(false);
            return;
        }

        nameText.text = objectPrefab.name;
        descriptionText.text = objectPrefab.uiDescription;
        costText.text = "$" + objectPrefab.cost.ToString();
        icon.sprite = objectPrefab.uiIcon;
    }

    /// <summary>
    /// Update the player controls script with a reference to the attached building prefab
    /// </summary>
    public void SelectBuilding()
    {
        // Don't allow player to select building if they don't have enough money
        if (gameManager.money < objectPrefab.cost)
        {
            return;
        }

        PlayerControls.instance.SpawnObject(objectPrefab.gameObject);
    }
}
