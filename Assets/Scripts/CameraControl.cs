using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            float z = Camera.main.transform.position.z - .5f;
            float x = Camera.main.transform.position.x + .5f;

            Vector3 CamPos = new Vector3(x, Camera.main.transform.position.y, z);

            Camera.main.transform.position = CamPos;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            float z = Camera.main.transform.position.z + .5f;
            float x = Camera.main.transform.position.x - .5f;

            Vector3 CamPos = new Vector3(x, Camera.main.transform.position.y, z);

            Camera.main.transform.position = CamPos;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            float z = Camera.main.transform.position.z + .5f;
            float x = Camera.main.transform.position.x + .5f;

            Vector3 CamPos = new Vector3(x, Camera.main.transform.position.y, z);

            Camera.main.transform.position = CamPos;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            float z = Camera.main.transform.position.z - .5f;
            float x = Camera.main.transform.position.x - .5f;

            Vector3 CamPos = new Vector3(x, Camera.main.transform.position.y, z);

            Camera.main.transform.position = CamPos;
        }
    }
}
