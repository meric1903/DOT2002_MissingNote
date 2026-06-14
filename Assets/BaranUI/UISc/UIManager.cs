using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class UIManager : MonoBehaviour
{
    public GameObject startMenu, settingsMenu, PauseMenu, settingsGameMenu;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OpenStartMenu();
    }



    // Update is called once per frame
    void Update()
    {

    }


    public void OpenStartMenu()
    {
        startMenu.SetActive(true);
        settingsMenu.SetActive(false);
        PauseMenu.SetActive(false);
        settingsGameMenu.SetActive(false);
        Time.timeScale = 0;
    }



    public void OpenSettingsMenu()
    {
        startMenu.SetActive(false);
        settingsMenu.SetActive(true);
        PauseMenu.SetActive(false);
        settingsGameMenu.SetActive(false);
        Time.timeScale = 0;
    }

    public void OpenPauseMenu()
    {
        startMenu.SetActive(false);
        settingsMenu.SetActive(false);
        PauseMenu.SetActive(true);
        settingsGameMenu.SetActive(false);
        Time.timeScale = 0;
    }

    public void OpenSettingsGameMenu()
    {
        startMenu.SetActive(false);
        settingsMenu.SetActive(false);
        PauseMenu.SetActive(false);
        settingsGameMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void InGame()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void ExitGame()
    {
        Application.Quit();
    }


}
