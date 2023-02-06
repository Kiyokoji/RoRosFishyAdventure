using System;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using FMODUnity;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class BookManager : MonoBehaviour
{
    private static bool gamePaused;

    private PlayerInputActions inputActions;

    [Header("Menus")]
    public GameObject settingsMenu;
    public GameObject mapMenu;
    public GameObject stuffMenu;

    public LevelMusic levelMusic;
    
    private bool isPLaying = false;

    private BookManager.MenuType currentMenu = BookManager.MenuType.Settings;

    private void OnEnable()
    {
        inputActions = new PlayerInputActions();
        inputActions.Input.Enable();

        inputActions.Input.Pause.performed += Pause_performed;
        inputActions.Input.Book.performed  += Book_performed;
        inputActions.Input.Left.performed  += Left_Scroll;
        inputActions.Input.Right.performed += Right_Scroll;
    }

    private void OnDisable()
    {
        inputActions.Input.Disable();
    }
    
    private void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName == "StarMenu")
        {
            levelMusic.Stop();
            isPLaying = false;
        }

        if (!isPLaying && sceneName != "StarMenu")
        {
            levelMusic.Play();
            isPLaying = true;
        }
        
    }

    private enum MenuType
    {
        Settings,
        Map,
        Stuff
    }

    private void ChangeMenu(MenuType menuType)
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.Paused);
        
        switch (menuType)
        {
            case MenuType.Map:
                Pause();
                Map();
                break;
            case MenuType.Settings:
                Pause();
                Settings();
                break;
            case MenuType.Stuff:
                Pause();
                Stuff();
                break;
        }
    }
    
    private void Pause_performed(InputAction.CallbackContext obj)
    {
        gamePaused = !gamePaused;

        if (gamePaused)
        {
            ChangeMenu(MenuType.Settings);

        } else
        {
            CloseMenus();
        }
    }

    private void Book_performed(InputAction.CallbackContext obj)
    {
        gamePaused = !gamePaused;

        if (gamePaused && (currentMenu != MenuType.Settings || currentMenu != MenuType.Stuff))
        {
            ChangeMenu(MenuType.Map);
        }
        else
        {
            CloseMenus();
        }
    }

    private void Left_Scroll(InputAction.CallbackContext obj)
    {
        if (!gamePaused) return;

        switch (currentMenu)
        {
            case MenuType.Map:
                ChangeMenu(MenuType.Settings);
                break;
            case MenuType.Stuff:
                ChangeMenu((MenuType.Map));
                break;
        }
    }
    
    private void Right_Scroll(InputAction.CallbackContext obj)
    {
        if (!gamePaused) return;

        switch (currentMenu)
        {
            case MenuType.Settings:
                ChangeMenu(MenuType.Map);
                break;
            case MenuType.Map:
                ChangeMenu((MenuType.Stuff));
                break;
        }
    }

    private void Settings()
    {
        currentMenu = MenuType.Settings;

        settingsMenu.SetActive(true);
        mapMenu.SetActive(false);
        stuffMenu.SetActive(false);
    }

    private void Map()
    {
        currentMenu = MenuType.Map;
        
        mapMenu.SetActive(true);
        settingsMenu.SetActive(false);
        stuffMenu.SetActive(false);
    }

    private void Stuff()
    {
        currentMenu = MenuType.Stuff;
        
        stuffMenu.SetActive(true);
        settingsMenu.SetActive(false);
        mapMenu.SetActive(false);
    }

    private void CloseMenus()
    {
        UnPause();
        
        gamePaused = false;
        
        settingsMenu.SetActive(false);
        mapMenu.SetActive(false);
        stuffMenu.SetActive(false);

        GameManager.Instance.UpdateGameState(GameManager.GameState.Moving);
    }

    private void Pause()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Pause Menu", 1);
    }

    private void UnPause()
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Pause Menu", 0);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        CloseMenus();
    }
    
    public void MainMenu()
    {
        SceneManager.LoadScene("StarMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Resume()
    {
        CloseMenus();
    }
}
