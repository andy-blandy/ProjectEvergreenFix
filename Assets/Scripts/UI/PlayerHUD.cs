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
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("Resource UI: Values")]
    public TextMeshProUGUI envImpactText;
    public TextMeshProUGUI populationText;
    public TextMeshProUGUI happinessText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI powerText;

    [Header("Resource UI: Visuals")]
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color warningColor;
    [SerializeField] private Color dangerColor;
    [SerializeField] private Color goodColor;

    [Header("Editing")]
    public bool isEditOpen;
    public GameObject editingMenu;
    public GameObject editingMenuButton;
    public GameObject editingMenuPopup;

    [Header("Placing")]
    public bool isPlaceOpen;
    public GameObject placeMenu;

    [Header("Audio")]
    public AudioSource missionsMenuSFX;
    public AudioSource editMenuSFX;
    public AudioSource buttonSFX;

    [Header("Missions")]
    public bool isMissionsOpen;
    public GameObject missionsMenu;
    public GameObject MissionsButton;

    public static PlayerHUD instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SetMenus();
    }

    void Update()
    {
        // Update UI
        UpdateResourceGUI();
    }

    void SetMenus()
    {
        missionsMenu.SetActive(false);
        editingMenuPopup.SetActive(false);
        placeMenu.SetActive(false);

        isMissionsOpen = false;
        isEditOpen = false;
        isPlaceOpen = false;
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
        populationText.text = gameManager.population.ToString();

        /// Happiness
        happinessText.text = gameManager.happiness.ToString();

        /// Power
        powerText.text = gameManager.availablePower.ToString();

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

    public void SwapMenu(GameObject chosenButton)
    {
        string chosenMenu = chosenButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;

        switch (chosenMenu) 
        {
            case "Place":
                ShowPlaceMenu(true);
                PlayerControls.instance.SwitchMode("place");
                break;
            case "Move":
                ShowPlaceMenu(false);
                PlayerControls.instance.SwitchMode("move");
                break;
            case "Destroy":
                ShowPlaceMenu(false);
                PlayerControls.instance.SwitchMode("destroy");
                break;
            default:
                break;
        }

        SwapButtons(chosenButton, editingMenuButton);
        ShowEditMenu();
    }

    public void SwapButtons(GameObject buttonOne, GameObject buttonTwo)
    {
        Image imageOne = buttonOne.GetComponent<Image>();
        Image imageTwo = buttonTwo.GetComponent<Image>();
        TextMeshProUGUI textOne = buttonOne.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI textTwo = buttonTwo.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        Sprite tempSprite = imageOne.sprite;
        string tempText = textOne.text;
        imageOne.sprite = imageTwo.sprite;
        textOne.text = textTwo.text;
        imageTwo.sprite = tempSprite;
        textTwo.text = tempText;
    }


    public void ShowMissionsMenu()
    {
        if (isMissionsOpen)
        {
            missionsMenu.SetActive(false);

            // Audio
            buttonSFX.Play();
        } else
        {
            missionsMenu.SetActive(true);

            // Audio
            missionsMenuSFX.Play();

        }

        isMissionsOpen = !isMissionsOpen;
    }

    public void ShowEditMenu()
    {
        if (!isEditOpen)
        {
            editingMenuPopup.SetActive(true);

            // Audio
            editMenuSFX.Play();

        } else
        {
            editingMenuPopup.SetActive(false);

            // Audio
            buttonSFX.Play();
        }

        isEditOpen = !isEditOpen;
    }

    public void ShowPlaceMenu(bool open)
    {
        placeMenu.SetActive(open);

        isPlaceOpen = !isPlaceOpen;
    }

    public void HideAllMenus()
    {
        if (missionsMenu.activeInHierarchy == true)
        {
            missionsMenu.SetActive(false);
        }
        if (editingMenu.activeInHierarchy == true)
        {
            editingMenu.SetActive(false);
        }
        if (editingMenuPopup.activeInHierarchy == true)
        {
            editingMenuPopup.SetActive(false);
        }
        if (placeMenu.activeInHierarchy == true)
        {
            placeMenu.SetActive(false);
        }
    }
}
