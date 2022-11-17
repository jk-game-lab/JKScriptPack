// Updated to work with Unity's new Input System.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class PauseMenu1 : MonoBehaviour {

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

#if ENABLE_INPUT_SYSTEM
    public Key PauseKey = Key.None;
#else
    public KeyCode PauseKey = KeyCode.None;
#endif

    // Update is called once per frame
    void Update () {

#if ENABLE_INPUT_SYSTEM
        if (PauseKey != Key.None && Keyboard.current[PauseKey].wasPressedThisFrame)
#else
        if (Input.GetKeyDown(PauseKey))
#endif
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
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void LoadLevel1()
    {
        Time.timeScale = 2f;
        SceneManager.LoadScene("Level1");
    }

    public void LoadLevel2()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level2");
    }

    public void LoadLevel3()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level3");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
