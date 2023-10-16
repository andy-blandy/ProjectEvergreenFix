/*
 * Written by Andrew Bland
 * @andy_blandy in Discord
 * 
 * Description:
 * This script is how the player will interact with the game world.
 * Currently, it only supports two modes: Destroy, and Place.
 * Only implemented in the TutorialPrototype scene.
 * 
 * TO-DO:
 * - Add a timer for destroying objects
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    // We need a reference to the gameCamera to shoot raycasts out of.
    // This will allow the player to interact with objects on the grid.
    private Camera gameCamera;

    [Header("States")]
    public string currentControlMode;
    public bool isPlacingObject;

    [Header("Place Mode")]
    public GameObject selectedObject;
    [SerializeField] private LayerMask groundLayer;
    [Range(1, 90)] public int rotationAmount = 45;

    [Header("GUI")]
    public GameObject placeMenu;
    public GameObject statChangePrefab;

    [Header("Audio")]
    public AudioSource buildSFX;
    public AudioSource destroySFX;
    public AudioSource buttonSFX;

    // Singleton
    public static PlayerControls instance;
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gameCamera = GameManager.instance.gameCamera;
        currentControlMode = "move";
    }

    void Update()
    {
        // When left mouse clicked...
        if (Input.GetMouseButtonDown(0))
        {
            DoAction();
        }

        // Call a different update method depending on the current mode
        switch (currentControlMode)
        {
            case "place":
                PlaceModeUpdate();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Handles building placement. If a building is currently selected, it will follow the player's cursor.
    /// </summary>
    void PlaceModeUpdate()
    {
        if (isPlacingObject)
        {
            // Shoot a raycast to find where on the ground the mouse is
            // This will only hit objects with their layer property set to "Ground"
            RaycastHit rayHit;
            Physics.Raycast(gameCamera.ScreenPointToRay(Input.mousePosition), out rayHit, Mathf.Infinity, groundLayer);

            // Set the position of the selected building to the player's cursor (found with raycast)
            if (rayHit.collider != null &&
                rayHit.collider.gameObject.TryGetComponent<Tile>(out Tile selectedTile))
            {
                selectedObject.transform.position = selectedTile.tileCenter;
            }

            // If Z or C are pressed, rotate the building
            if (Input.GetKeyDown(KeyCode.Z))
            {
                selectedObject.transform.Rotate(new Vector3(0f, rotationAmount, 0f));
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                selectedObject.transform.Rotate(new Vector3(0f, rotationAmount * -1, 0f));
            }

            // Deselect the building when backspace is pressed
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                DeselectObject();
            }
        }
    }

    /// <summary>
    /// Change the current mode to the given choice. Is called by the buttons in the PlayerHUD.
    /// TODO: Only allow certain options to be passed through (destroy, place, move)
    /// </summary>
    /// <param name="modeChoice">Possible options: destroy, place, move</param>
    public void SwitchMode(string modeChoice)
    {
        currentControlMode = modeChoice;

        if (modeChoice != "place")
        {
            placeMenu.SetActive(false);
        } else
        {
            placeMenu.SetActive(true);
        }

        // If needed, delete selected building
        if (isPlacingObject)
        {
            DeselectObject();
        }

        // Update UI
        PlayerHUD.instance.currentModeText.text = currentControlMode;
    }

    /// <summary>
    /// Do an action depending on the current control mode.
    /// </summary>
    void DoAction()
    {
        switch (currentControlMode)
        {
            case "destroy":
                DeleteObject();
                break;
            case "place":
                if (isPlacingObject)
                {
                    PlaceObject();
                }
                break;
            default:
                break;
        }
    }


    /// <summary>
    /// Deletes the object the player clicks on if the gameobject has the tag "Destructable"
    /// </summary>
    void DeleteObject()
    {
        // Audio
        buttonSFX.Play();

        // Shoot a raycast out from the camera to find the object the player is pointing at
        RaycastHit rayHit;
        Physics.Raycast(gameCamera.ScreenPointToRay(Input.mousePosition), out rayHit, Mathf.Infinity);

        // If the object has the tag "Destructable", deactivate it.
        if (rayHit.collider != null &&
            rayHit.collider.gameObject.tag == "Tree")
        {
            // Hide the object
            rayHit.collider.gameObject.SetActive(false);

            // Adjust the environmental impact
            GameManager.instance.envImpact += 1;
            GameObject statChange = Instantiate(statChangePrefab, rayHit.point, Quaternion.identity);
            statChange.GetComponent<StatChangePopup>().SetArrow(true, "EI");

            // Audio
            destroySFX.Play();
        }

        if (rayHit.collider != null &&
            rayHit.collider.gameObject.tag == "Building")
        {
            // Hide the object
            rayHit.collider.gameObject.SetActive(false);

            // Adjust the environmental impact
            GameManager.instance.envImpact -= 1;
            GameObject statChange = Instantiate(statChangePrefab, rayHit.point, Quaternion.identity);
            statChange.GetComponent<StatChangePopup>().SetArrow(false, "EI");

            // Audio
            destroySFX.Play();
        }

        if (rayHit.collider != null &&
            rayHit.collider.gameObject.tag == "Road")
        {
            // Hide the object
            rayHit.collider.gameObject.SetActive(false);

            // Adjust the environmental impact
            //GameManager.instance.envImpact -= 1;
            //GameObject statChange = Instantiate(statChangePrefab, rayHit.point, Quaternion.identity);
            //statChange.GetComponent<StatChangePopup>().SetArrow(false, "EI");

            // Audio
            destroySFX.Play();
        }
    }

    /// <summary>
    /// Instantiate the desired building
    /// </summary>
    /// <param name="objectToSpawn"></param>
    public void SpawnObject(GameObject objectToSpawn)
    {
        isPlacingObject = true;

        // Spawn building off screen
        selectedObject = Instantiate(objectToSpawn, new Vector3(0f, -100f, 0f), Quaternion.identity);

        // Get reference to building script
        PlaceableObject objectScript = selectedObject.GetComponent<PlaceableObject>();

        // Set building model to guide
        objectScript.SetModelToGuide();
        objectScript.isPaidFor = false;
    }

    /// <summary>
    /// Place the selected building on the map
    /// </summary>
    void PlaceObject()
    {
        // Audio
        buttonSFX.Play();

        PlaceableObject objectScript = selectedObject.GetComponent<PlaceableObject>();

        // Same raycast code as before
        RaycastHit rayHit;
        Physics.Raycast(gameCamera.ScreenPointToRay(Input.mousePosition), out rayHit, Mathf.Infinity, groundLayer);

        // Do nothing if the mouse is not clicking the ground, or if the building is currently colliding with another object
        if (rayHit.collider == null || objectScript.isColliding)
        {
            return;
        }

        // Set building model to final model
        objectScript.PlaceObject();

        // If the object is a road, give it to the road manager.
        // This will deal with the logic of connecting buildings to the town square (eventually...)
        if (selectedObject.TryGetComponent<Road>(out Road road))
        {
            RoadManager.instance.AddRoad(road);
        }

        // Stop tracking building
        isPlacingObject = false;
        selectedObject = null;

        // If this is the first time placing the building, pay the cost of it
        // Ideally this will allow the player to eventually move around buildings they've placed, without having to pay for it each time
        if (!objectScript.isPaidFor)
        {
            PayForBuilding(objectScript);
        }

        /*
         * Insert code to connect to grid manager
         */

        // Audio
        buildSFX.Play();
    }

    void PayForBuilding(PlaceableObject objectScript)
    {
        // Remove the cost of the building from the player's money
        GameManager.instance.money -= objectScript.cost;

        /*
         * Insert code for GUI here
         * i.e. "-$50" text appears and slowly floats up + fades away, with a 'ca-ching' sound effect
         */

        // Update the building's logic
        objectScript.isPaidFor = true;
    }

    /// <summary>
    /// Removes the currently selected object from the scene
    /// </summary>
    void DeselectObject()
    {
        if (selectedObject != null)
        {
            Destroy(selectedObject);
        }

        isPlacingObject = false;
        selectedObject = null;
    }

}
