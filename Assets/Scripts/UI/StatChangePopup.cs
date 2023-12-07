/*
 * Written by Andrew Bland
 * @andy_blandy on Discord
 * 
 * Description:
 * This controls the pop-up for stat changes
 * i.e. when a building is placed, an icon appears near the building showing how much money the player spent
 * This makes the icon float up and fade away.
 * Pop-up has an arrow pointing upwards or downwards depending on whether the stat increased or decreased.
 * Pop-up also has text describing what stat changed (should be changed an icon eventually)
 * I'm not in love with this script, so feel free to tear it to shreds and rebuild. Make sure to adjust the references to it in PlayerControls if you do.
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatChangePopup : MonoBehaviour
{
    [Header("GUI")]
    [Tooltip("Set the first sprite to the up arrow, second to the down arrow")]public Sprite[] arrowSpriteReferences;
    public Image arrowImage;
    public TextMeshProUGUI statNameText;
    private CanvasGroup canvasGroup;

    [Header("Animation Settings")]
    public float moveSpeed = 0.1f;
    public float totalMoveAmount = 1f;
    
    void Awake()
    {
        // Get ref to canvas group. This is important for controling the alpha of the pop-up, to make it fade away
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        Camera camera = Camera.main;
        transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
    }

    /// <summary>
    /// Set's the graphics of the pop-up
    /// </summary>
    /// <param name="isStatIncreasing">Set to 'true' for an upward arrow, set to 'false' for a downward arrow</param>
    /// <param name="statName"></param>
    public void SetArrow(bool isStatIncreasing, string statName)
    {
        // Set the visuals of the pop-up
        if (isStatIncreasing)
        {
            // Set sprite to upward arrow
            arrowImage.sprite = arrowSpriteReferences[0];
        } else
        {
            // Set sprite to downward arrow
            arrowImage.sprite = arrowSpriteReferences[1];
        }
        statNameText.text = statName;

        // Start the animation.
        StartCoroutine(AnimatePopup());
    }

    /// <summary>
    /// Animate the popup moving upwards and slowly fading away.
    /// "totalMoveAmount" determines how high on the y-axis the pop-up will move.
    /// "moveSpeed" determines how fast the animation is. Lower the value for a faster animation.
    /// </summary>
    /// <returns></returns>
    IEnumerator AnimatePopup()
    {
        Vector3 goalDestination = transform.position + new Vector3(0f, totalMoveAmount, 0f);
        Vector3 moveAmount = goalDestination - transform.position;
        moveAmount *= 0.1f;

        while (transform.position != goalDestination)
        {
            // Move the popup
            // There's probably a better way to do this?
            transform.position += moveAmount;

            // Changes the alpha of the object to have it 'fade'
            canvasGroup.alpha -= 0.1f;

            // Wait a couple milliseconds
            yield return new WaitForSeconds(moveSpeed);
        }

        // Completely remove the pop-up after the animation is over
        Destroy(gameObject);
    }
}
