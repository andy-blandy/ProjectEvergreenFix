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
    public MeshRenderer[] modelMeshRenderer;
    public Material[] placedMaterial;
    public Material errorMaterial;
    public Material guideMaterial;

    [Header("Collision Detection")]
    public bool isColliding;
    private Collider currentCollider;

    void Awake()
    {
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetModelToGuide()
    {
        foreach (MeshRenderer meshRenderer in modelMeshRenderer)
        {
            meshRenderer.SetMaterials(new List<Material>() { guideMaterial });
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
        foreach (MeshRenderer meshRenderer in modelMeshRenderer)
        {
            meshRenderer.SetMaterials(new List<Material>() { errorMaterial });
        }
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
            foreach (MeshRenderer meshRenderer in modelMeshRenderer)
            {
                meshRenderer.SetMaterials(new List<Material>() { guideMaterial });
            }
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
        for (int i = 0; i < modelMeshRenderer.Length; i++)
        {
            modelMeshRenderer[i].SetMaterials(new List<Material>() { placedMaterial[i] });
        }

        OnPlace();
    }

    /// <summary>
    /// Override this function in a child script to have it run unique logic after being placed
    /// </summary>
    public virtual void OnPlace()
    {

    }
}
