using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    SceneLoader loader;

    public SpriteRenderer interaction;
    private PlayerInputActions playerInputActions;
    private bool entered = false;

    public SceneLoader.Level SceneToLoad = SceneLoader.Level.Menu;

    private void OnEnable()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interacted;
    }

    private void OnDisable()
    {
        playerInputActions.Player.Disable();
    }

    private void Interacted(InputAction.CallbackContext obj)
    {
        if (entered)
        {
            loader.LoadNextLevel();
            GameManager.instance.UpdateGameState(GameManager.GameState.Idle);
        }
    }

    void Start()
    {
        interaction.enabled = false;
        loader = FindObjectOfType<SceneLoader>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Entered();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Left();
        }
    }

    private void Entered()
    {
        interaction.enabled = true;
        entered = true;
    }

    private void Left()
    {
        interaction.enabled = false;
        entered = false;
    }
}
