/*
 * Written by Andrew
 * @andy_blandy on Discord
 * 
 * Description:
 * Each tile in the game will have this class attached to it
 * All this does right now is return a spot on the top center of the tile so that buildings can snap to it.
 * At some point, would like to add functionality for the tileType.
 * From what I understand, having this class inherit from MonoBehaviour will slow things down in the future... so will probably need to be optimized
 * Maybe use Unity's tilemap system instead?
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Tile : MonoBehaviour
{
    [Header("Grid Snapping")]
    public Vector3 tileCenter;
    public string tileType;

    // Start is called before the first frame update
    void Start()
    {
        if (Application.isPlaying)
        {
            // Play logic
            // Get the center of the tile, which will be halfway up from the center of the object
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

    void SetTIleColor()
    {
        //Material tempMat = ground.GetComponent<MeshRenderer>().material;
        //tempMat.color = tileType[x, y] switch
        //{
        //    "forest" => new Color(20 / 255f, 155 / 255f, 20 / 255f),
        //    "paved" => new Color(180 / 255f, 180 / 255f, 180 / 255f),
        //    "dirt" => new Color(158 / 255f, 101 / 255f, 66 / 255f),
        //    "plains" => new Color(101 / 255f, 198 / 255f, 101 / 255f),
        //    "river" => new Color(91 / 255f, 146 / 255f, 237 / 255f),
        //    _ => Color.white
        //};
        //float tileDifferentiator = 1 - ((x + (y % 2)) % 2 * 0.15f);
        //tempMat.color *= tileDifferentiator;
    }


    public bool IsTileBuildable()
    {
        return tileType switch
        {
            "forest" => false,
            "river" => false,
            _ => true
        };
    }
}
