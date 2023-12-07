using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Submenu : MonoBehaviour
{
    public GameObject menu;
    public bool isOpened;

    public void OpenMenu()
    {
        if (isOpened)
        {
            menu.SetActive(false);
            isOpened = false;
        } else
        {
            menu.SetActive(true);
            isOpened = true;
        }
    }
}
