using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;


    public AudioMixer audioMixer;
    public float masterVol;
    public float musicVol;
    public float sfxVol;

    public GameObject pauseMenuUI;
    public GameObject settingsMenu;

    public static PauseMenu instance;
    void Awake()
    {
        instance = this;
    }

    public void OpenPauseMenu()
    {
        pauseMenuUI.SetActive(true);
    }

    public void CloseAllMenus()
    {
        pauseMenuUI.SetActive(false);
        settingsMenu.SetActive(false);
    }

    public void Resume()
    {
        GameManager.instance.PauseGame();
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

    public void SetMasterVol(Slider slider)
    {
        masterVol = slider.value;
        audioMixer.SetFloat("Master", masterVol);
    }

    public void SetMusicVol(Slider slider)
    {
        musicVol = slider.value;
        audioMixer.SetFloat("Music", musicVol);
    }

    public void SetSFXVol(Slider slider) 
    { 
        sfxVol = slider.value;
        audioMixer.SetFloat("SFX", sfxVol);
    }
}
