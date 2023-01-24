using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseUIHandler : MenuUIHandler
{
    [SerializeField] GameObject pauseMenu;
    public static bool isPaused = false;

    public void RestartGame()
    {
        ResumeGame();
        StartGame();
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        AudioListener.pause = isPaused;
        pauseMenu.SetActive(isPaused);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        AudioListener.pause = isPaused;
        pauseMenu.SetActive(isPaused);
    }
}
