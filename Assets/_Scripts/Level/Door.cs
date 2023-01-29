using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;

public class Door : MonoBehaviour
{
    private SceneLoader loader;
    private Animator anim;

    public EventReference doorSound;
    public SpriteRenderer interaction;
    private PlayerInputActions playerInputActions;
    private bool entered = false;

    public SceneLoader.Level sceneToLoad = SceneLoader.Level.Menu;

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
        if (!entered) return;
        
        loader.LoadNextLevel();
        GameManager.Instance.UpdateGameState(GameManager.GameState.Idle);
    }

    private void Start()
    {
        anim = GetComponent<Animator>();

        interaction.enabled = false;
        loader = FindObjectOfType<SceneLoader>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1") || collision.CompareTag("Player2"))
        {
            Entered();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1") || collision.CompareTag("Player2"))
        {
            Left();
        }
    }

    private void Entered()
    {
        FMODUnity.RuntimeManager.PlayOneShot(doorSound);
        
        interaction.enabled = true;
        anim.SetBool("Door", true);
        entered = true;
    }

    private void Left()
    {
        interaction.enabled = false;
        anim.SetBool("Door", false);
        entered = false;
    }
}
