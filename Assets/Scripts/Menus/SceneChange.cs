using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    // Audio
    public AudioSource buttonSFX;

    public void LoadScene(int sceneBuildIndex)
    {
        buttonSFX.Play();
        if (sceneBuildIndex == 99)
        {
            Debug.Log("Game Closed");
            Application.Quit();
        }
        else {
            SceneManager.LoadScene(sceneBuildIndex);
        }
    }
}
