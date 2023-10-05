/*
 * Written by Andrew Bland
 * @andy_blandy on Discord
 * 
 * Description:
 * Controls the Player HUD.
 * Only implemented in TutorialProtoype atm.
 * 
 * To Add:
 *  - More menu options
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [Header("Resource UI: Values")]
    public TextMeshProUGUI envImpactText;
    public TextMeshProUGUI populationText;
    public TextMeshProUGUI happinessText;
    public TextMeshProUGUI moneyText;

    [Header("Resource UI: Visuals")]
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color warningColor;
    [SerializeField] private Color dangerColor;
    [SerializeField] private Color goodColor;

    [Header("Editing")]
    public GameObject editingMenu;
    public TextMeshProUGUI currentModeText;

    public static PlayerHUD instance;
    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        // Update UI
        UpdateResourceGUI();

        // Open the editing menu if "Escape" is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (editingMenu.activeSelf)
            {
                editingMenu.SetActive(false);
                PlayerControls.instance.SwitchMode("move");
            } else
            {
                editingMenu.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Update the resource UI with the values from the GameManager
    /// </summary>
    void UpdateResourceGUI()
    {
        GameManager gameManager = GameManager.instance;

        /// Environmental Impact
        envImpactText.text = gameManager.envImpact.ToString();

        // Set the color based on the level of environmental impact
        // These are placeholder values, should be changed at some point
        if (gameManager.envImpact >= 50 && gameManager.envImpact < 75)
        {
            envImpactText.color = warningColor;
        } else if (gameManager.envImpact >= 75)
        {
            envImpactText.color = dangerColor;
        } else
        {
            envImpactText.color = defaultColor;
        }

        /// Population
        populationText.text = gameManager.citizens.ToString();

        /// Happiness
        happinessText.text = gameManager.happiness.ToString();

        // Set the color of the happiness value based on the happiness level
        // If negative, set to the danger color. If positive, set to the good color.
        if (gameManager.happiness < 0)
        {
            happinessText.color = dangerColor;
        }
        else if (gameManager.happiness == 0)
        {
            happinessText.color = defaultColor;
        } else if (gameManager.happiness > 0)
        {
            happinessText.color = goodColor;
        }

        /// Money
        moneyText.text = gameManager.money.ToString();
    }

}
