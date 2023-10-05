/*
 * Written by Andrew Bland
 * @andy_blandy on Discord
 * 
 * Description:
 * This is a proof-of-concept prototype version of the building class for the tutorial level I'm making
 * This probably shouldn't be used in the final game without major QoL tweaks
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeBuilding : MonoBehaviour
{
    public string buildingName;

    public bool isColliding;
    private Collider currentCollider;

    [Header("States")]
    public bool isPaidFor;
    public bool isConnectedToRoads;

    [Header("Stats")]
    public int cost;

    [Header("UI")]
    public string uiDescription;
    public Sprite uiIcon;

    [Header("Graphics")]
    public MeshRenderer modelMeshRenderer;
    public Material buildingMaterial;
    public Material errorMaterial;
    public Material guideMaterial;

    public void SetModelToGuide()
    {
        modelMeshRenderer.SetMaterials(new List<Material>() { guideMaterial });
    }

    public void SetModelToPlaced()
    {
        // Update collision stuff
        GetComponent<BoxCollider>().isTrigger = false;
        Destroy(GetComponent<Rigidbody>());

        modelMeshRenderer.SetMaterials(new List<Material>() { buildingMaterial });
    }

    // If the building's collider is touching another collider, don't allow the player to place the object
    void OnTriggerEnter(Collider other)
    {
        currentCollider = other;
        isColliding = true;
        modelMeshRenderer.SetMaterials(new List<Material>() { errorMaterial });
    }

    // If the building's collider exits another collider, allow the player to place the object
    private void OnTriggerExit(Collider other)
    {
        if (other == currentCollider)
        {
            currentCollider = null;
            isColliding = false;
            modelMeshRenderer.SetMaterials(new List<Material>() { guideMaterial });
        }
    }
}
