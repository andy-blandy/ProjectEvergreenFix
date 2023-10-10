using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    public string objectName;

    public bool isColliding;
    private Collider currentCollider;

    [Header("Stats")]
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

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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

    // If the building's collider is touching another collider, don't allow the player to place the object
    void OnTriggerEnter(Collider other)
    {
        // Do nothing if the building has already been placed
        if (isPlaced)
        {
            return;
        }

        currentCollider = other;
        isColliding = true;
        foreach (MeshRenderer meshRenderer in modelMeshRenderer)
        {
            meshRenderer.SetMaterials(new List<Material>() { errorMaterial });
        }

        Debug.Log(other.gameObject.name);
    }

    // If the building's collider exits another collider, allow the player to place the object
    private void OnTriggerExit(Collider other)
    {
        // Do nothing if the building has already been placed
        if (isPlaced)
        {
            return;
        }

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
