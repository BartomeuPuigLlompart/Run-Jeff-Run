using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public Camera camara;

    bool soundOn = true;

    [SerializeField] AudioSource musicSource;

    // Update is called once per frame
    void Update()
    {
        soundOn = (PlayerPrefs.GetFloat("SoundVolume", 0.5f) > 0.0f);

        if (Input.GetKeyDown(KeyCode.Escape))
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
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        
        //camara=GameObject.Find("Main Camera").GetComponent<Camera>();
        
    }
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        pauseMenuUI.SetActive(false);
        SceneManager.LoadScene(1); //Menu scene
       
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    

    public void SetVolume()
    {
        soundOn = !soundOn;

        PlayerPrefs.SetFloat("SoundVolume", (soundOn) ? 0.5f : 0f);

        if(musicSource) musicSource.volume = PlayerPrefs.GetFloat("SoundVolume", 0.5f) * 0.25f;
    }
      
}
