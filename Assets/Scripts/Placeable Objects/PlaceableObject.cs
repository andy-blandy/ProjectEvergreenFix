/*
 * Written by Andrew
 * @andy_blandy on Discord
 * 
 * Description:
 * Every object that can be placed will inherit from this class
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    [Header("Properties")]
    public string objectName;
    public int cost;

    [Header("UI")]
    public string uiDescription;
    public Sprite uiIcon;

    [Header("States")]
    public bool isPaidFor;
    public bool isPlaced;

    [Header("Graphics")]
    public GameObject model;
    public MeshRenderer[] modelMeshRenderers;
    public List<List<Material>> placedMaterials;
    public List<List<Material>> guideMaterials;
    public List<List<Material>> errorMaterials;

    [Header("Collision Detection")]
    public bool isColliding;
    private Collider currentCollider;

    void Awake()
    {
        model = transform.GetChild(0).gameObject;
        modelMeshRenderers = model.GetComponentsInChildren<MeshRenderer>();

        placedMaterials = new List<List<Material>>();
        guideMaterials = new List<List<Material>>();
        errorMaterials = new List<List<Material>>();

        // This will probably cause a lot of problems eventually
        for (int i = 0; i < modelMeshRenderers.Length; i++)
        {
            // Get the original materials of the model
            placedMaterials.Add(new List<Material>());
            modelMeshRenderers[i].GetMaterials(placedMaterials[i]);

            // Set-up the guide materials and error materials
            // These lists need the same number of materials as the placedMaterials list, which is why we use a for loop.
            guideMaterials.Add(new List<Material>());
            errorMaterials.Add(new List<Material>());
            for (int j = 0; j < placedMaterials[i].Count; j++)
            {
                guideMaterials[i].Add(BuildingManager.instance.guideMaterial);
                errorMaterials[i].Add(BuildingManager.instance.errorMaterial);
            }
        }



    }

    public void SetModelToGuide()
    {
        for (int i = 0; i < modelMeshRenderers.Length; i++)
        {
            modelMeshRenderers[i].SetMaterials(guideMaterials[i]);
        }
    }

    public void SetModelToError()
    {
        for (int i = 0; i < modelMeshRenderers.Length; i++)
        {
            modelMeshRenderers[i].SetMaterials(errorMaterials[i]);
        }
    }

    public void SetModelToPlaced()
    {
        for (int i = 0; i < modelMeshRenderers.Length; i++)
        {
            modelMeshRenderers[i].SetMaterials(placedMaterials[i]);
        }
    }

    #region Collision Detection
    void OnTriggerEnter(Collider other)
    {
        // Do nothing if the building has already been placed
        if (isPlaced)
        {
            return;
        }

        // If the building's collider is touching another collider, don't allow the player to place the object
        currentCollider = other;
        isColliding = true;

        SetModelToError();
    }

    private void OnTriggerExit(Collider other)
    {
        // Do nothing if the building has already been placed
        if (isPlaced)
        {
            return;
        }

        // If the building's collider exits another collider, allow the player to place the object
        if (other == currentCollider)
        {
            currentCollider = null;
            isColliding = false;
            SetModelToGuide();
        }
    }
    #endregion

    public void PlaceObject()
    {
        // Update states
        isPlaced = true;

        // Update collision stuff
        GetComponent<BoxCollider>().isTrigger = false;
        Destroy(GetComponent<Rigidbody>());

        // Update the model
        SetModelToPlaced();

        OnPlace();
    }

    /// <summary>
    /// Override this function in a child script to have it run unique logic after being placed
    /// </summary>
    public virtual void OnPlace()
    {

    }
}
