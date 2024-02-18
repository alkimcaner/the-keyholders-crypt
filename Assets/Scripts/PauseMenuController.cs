using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public FPSController fPSController;
    public Player player;
    public GameObject Menu;
    public bool isPaused = false;

    void Start()
    {
        Menu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0.0f;
        Menu.SetActive(true);
        fPSController.lookSpeed = 0;
        player.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1.0f;
        Menu.SetActive(false);
        fPSController.lookSpeed = 2;
        player.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
