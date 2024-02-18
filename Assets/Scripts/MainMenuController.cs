using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Town");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
    }
}
