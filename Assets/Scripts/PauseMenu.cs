using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [Header("Volume Settings")]
    //[SerializeField] private TMP_Text volumeTextValue;
    //[SerializeField] private Slider volumeSlider;

    private static bool gamePaused;

    private PlayerInputActions inputActions;

    public GameObject pauseMenuUI;
    public GameObject mapUI;
    public GameObject constellationUI;

    private void OnEnable()
    {
        inputActions = new PlayerInputActions();
        inputActions.Input.Enable();

        inputActions.Input.Pause.performed += Pause_performed;
        inputActions.Input.Book.performed  += Book_performed;
    }

    private void OnDisable()
    {
        inputActions.Input.Disable();
    }

    private void Pause_performed(InputAction.CallbackContext obj)
    {
        gamePaused = !gamePaused;

        if (gamePaused)
        {
            EnableMenu(0);

        } else
        {
            CloseMenus();
        }
    }

    private void Book_performed(InputAction.CallbackContext obj)
    {
        gamePaused = !gamePaused;

        if (gamePaused)
        {
            EnableMenu(1);
        }
        else
        {
            CloseMenus();
        }
    }

    private void EnableMenu(int menu)
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.Paused);

        switch (menu)
        {
            case 0:
                //Pause Menu
                pauseMenuUI.SetActive(true);
                mapUI.SetActive(false);
                constellationUI.SetActive(false);
                break;
            case 1:
                //Map Menu
                mapUI.SetActive(true);
                constellationUI.SetActive(false);
                pauseMenuUI.SetActive(false);
                break;
            case 2:
                //Constellation Menu
                mapUI.SetActive(true);
                constellationUI.SetActive(false);
                pauseMenuUI.SetActive(false);
                break;
        }
    }

    private void CloseMenus()
    {
        mapUI.SetActive(false);
        constellationUI.SetActive(false);
        pauseMenuUI.SetActive(false);

        GameManager.Instance.UpdateGameState(GameManager.GameState.Moving);
    }
    
    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
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
