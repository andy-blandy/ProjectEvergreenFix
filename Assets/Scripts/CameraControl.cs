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
        Rotate();
        UpdateCameraSpeed();
    }

    void Move()
    {
        Vector3 movementVector = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            movementVector.z += 1.0f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            movementVector.z -= 1.0f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            movementVector.x -= 1.0f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            movementVector.x += 1.0f;
        }

        transform.Translate(movementVector * movementSpeed * Time.deltaTime);
    }

    void Rotate()
    {
        Vector3 rotateAmount = new Vector3(0f, 10f, 0f);
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.Rotate(rotateAmount);
        }

        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            transform.Rotate(rotateAmount * -1);
        }
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
