using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private float movementSpeed;
    public float normalSpeed = 5.0f;
    public float quickSpeed = 10.0f;

    void Start()
    {
        movementSpeed = normalSpeed;
    }

    void Update()
    {
        Move();
        UpdateCameraSpeed();
    }

    void Move()
    {
        Vector3 movementVector = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            movementVector += transform.forward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            movementVector -= transform.forward;
        }

        if (Input.GetKey(KeyCode.A))
        {
            movementVector -= transform.right;
        }

        if (Input.GetKey(KeyCode.D))
        {
            movementVector += transform.right;
        }

        transform.Translate(movementVector * movementSpeed * Time.deltaTime);
    }

    void UpdateCameraSpeed()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            movementSpeed = quickSpeed;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            movementSpeed = normalSpeed;
        }
    }
}
