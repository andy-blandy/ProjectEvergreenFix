using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;


    public float bgVolume;
    public float volSFX;
    PlayerControls p = new PlayerControls();

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene("Title");
    }
    public void QuitGame()
    {
        Debug.Log("Game Closed");
        Application.Quit();
    }
    public void settings()
    {
        GUI.Label(new Rect(5, 170, 125, 30), "SFX Volume:");
        volSFX = GUI.HorizontalSlider(new Rect(115, 175, 125, 30), volSFX, 0.0f, 1.0f);
        setSFX(volSFX);
    }

    public void setSFX(float SFX)
    {
        p.buildSFX.GetComponent<AudioSource>().volume = SFX;
        p.destroySFX.GetComponent<AudioSource>().volume = SFX;
        p.buttonSFX.GetComponent<AudioSource>().volume = SFX;
    }
}
