using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Tile : MonoBehaviour
{
    public Vector3 tileCenter;
    public string tileType;

    // Start is called before the first frame update
    void Start()
    {
        if (Application.isPlaying)
        {
            // Play logic
            // Get the center of the top of the tile, which is halfway up from the center of the object
            tileCenter = transform.position + new Vector3(0f, (transform.localScale.y * 0.5f), 0f);
        } else
        {
            // Editor logic
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isPlaying)
        {
            // Play logic
        } else
        {
            // Editor Logic
            // Continuously update the tile center, to accomodate for scaling objects in edit mode
            tileCenter = transform.position + new Vector3(0f, (transform.localScale.y * 0.5f), 0f);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a sphere at the center of the tile, where objects will snap to.
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(tileCenter, 0.1f);
    }
}
