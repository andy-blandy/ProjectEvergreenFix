using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scene_change : MonoBehaviour
{
    // Audio
    public AudioSource buttonSFX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void NextScene()
    {
        buttonSFX.Play();
        SceneManager.LoadScene("SampleScene");
    }
    public void playTutorial()
    {
        buttonSFX.Play();
        SceneManager.LoadScene("TutorialPrototype");
    }
}
