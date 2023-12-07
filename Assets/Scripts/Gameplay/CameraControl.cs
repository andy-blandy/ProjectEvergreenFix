/*
 * Originally named "CameraMove", written by Brendan
 * Modified by Andrew Bland
 * 
 * Description:
 * This allows the player to move, rotate, and zoom the camera in and out
 * This isn't attached to the camera itself, but a CameraGimbal object that the camera is a child of
 * This allows for the camera to be rotated up and down without affecting the movement.
 * 
 * Control Scheme:
 * WASD - Move camera
 * Q/E - Rotate left/right
 * Scroll - Zoom in/out
 * Shift - Hold down to move faster. Release to return to normal speed.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Header("Movement")]
    public float normalMoveSpeed = 5.0f;
    public float quickMoveSpeed = 10.0f;
    private float movementSpeed;

    [Header("Rotation")]
    public float normalRotateSpeed = 30f;
    public float quickRotateSpeed = 60f;
    private float rotateSpeed;

    [Header("Zoom")]
    public float normalZoomSpeed = 0.5f;
    public float quickZoomSpeed = 1f;
    private float zoomSpeed;
    [Tooltip("x value is the min, y value is the max")] public Vector2 yZoomRange;

    void Start()
    {
        // Initialize all of the camera's speeds to their normal speeds.
        movementSpeed = normalMoveSpeed;
        rotateSpeed = normalRotateSpeed;
        zoomSpeed = normalZoomSpeed;
    }

    void Update()
    {
        Move();
        Rotate();
        Zoom();
        UpdateCameraSpeed();
    }

    void Move()
    {
        Vector3 movementVector = Vector3.zero;

        // Move along the z-axis
        if (Input.GetKey(KeyCode.W))
        {
            movementVector.z += 1.0f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            movementVector.z -= 1.0f;
        }

        // Move along the x-axis
        if (Input.GetKey(KeyCode.A))
        {
            movementVector.x -= 1.0f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            movementVector.x += 1.0f;
        }

        // Check to make sure camera is within map boundaries
        if (WithinBoundaries(transform.position + (movementVector * movementSpeed * Time.deltaTime)))
        {
            // Apply the movement to the camera gimbal
            transform.Translate(movementVector * movementSpeed * Time.deltaTime);
        }
    }

    void Rotate()
    {
        Vector3 rotateVector = Vector3.zero;
        
        // Rotate right
        if (Input.GetKey(KeyCode.E))
        {
            rotateVector = new Vector3(0, 1f, 0);
        }

        // Rotate left
        if (Input.GetKey(KeyCode.Q)) 
        {
            rotateVector = new Vector3(0, -1f, 0f);
        }

        // Apply rotation to the camera gimbal
        transform.Rotate(rotateVector * rotateSpeed * Time.deltaTime);
    }

    void Zoom()
    {
        // Get the mouse scroll wheel input
        float mouseScrollValue = Input.GetAxis("Mouse ScrollWheel");

        // If the player is moving the scroll wheel...
        if (mouseScrollValue != 0f)
        {
            Vector3 newPosition = transform.position;

            if (mouseScrollValue > 0)   // If scroll up...
            {
                // Do nothing if completely zoomed in
                if (newPosition.y <= yZoomRange[0])
                {
                    newPosition.y = yZoomRange[0];
                    return;
                }

                // Zoom in
                newPosition.y -= zoomSpeed;
                newPosition += transform.forward * zoomSpeed;
            }
            else if (mouseScrollValue < 0) // If scroll down...
            {
                // Do nothing if completely zoomed out
                if (newPosition.y >= yZoomRange[1])
                {
                    newPosition.y = yZoomRange[1];
                    return;
                }

                // Zoom out
                newPosition.y += zoomSpeed;
                newPosition -= transform.forward * zoomSpeed;
            }

            // Apply the zoom to the camera gimbal
            if (WithinBoundaries(newPosition))
            {
                transform.position = newPosition;
            }
        }
    }

    /// <summary>
    /// Changes the speed of the camera movement, depending on whether shift is held down or not
    /// </summary>
    void UpdateCameraSpeed()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            movementSpeed = quickMoveSpeed;
            rotateSpeed = quickRotateSpeed;
            zoomSpeed = quickZoomSpeed;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            movementSpeed = normalMoveSpeed;
            rotateSpeed = normalRotateSpeed;
            zoomSpeed = normalZoomSpeed;
        }
    }

    public bool WithinBoundaries(Vector3 result)
    {
        if (result.z > GameManager.instance.mapSize.z * 0.5f ||
    result.z < GameManager.instance.mapSize.z * -0.5f ||
    result.x > GameManager.instance.mapSize.x * 0.5f ||
    result.x < GameManager.instance.mapSize.x * -0.5f)
        {
            return false;
        }

        return true;
    }
}
