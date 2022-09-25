using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System;

public class GameManager : MonoBehaviour
{
    public GameState state;

    public static GameManager instance;
    private PlayerInputActions playerInputActions;

    public static event Action<GameState> gameStateChanged;

    private void Awake()
    {
        //Input
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.QuitGame.performed += QuitGame;

        //GameManager instance
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    private void Start()
    {
        UpdateGameState(GameState.Moving);
    }

    private void QuitGame(InputAction.CallbackContext obj)
    {
        Application.Quit();
    }

    public void UpdateGameState(GameState newState)
    {
        state = newState;

        switch (newState)
        {
            case GameState.Moving:
                //Player can move
                break;
            case GameState.Idle:
                //Player can't move
                break;
        }

        gameStateChanged?.Invoke(newState);
    }

    public enum GameState
    {
        //just placeholders for now
        Moving,
        Idle,
    }

}
