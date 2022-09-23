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
    public static GameManager instance;
    private PlayerInputActions playerInputActions;


    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.QuitGame.performed += QuitGame;

        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    private void QuitGame(InputAction.CallbackContext obj)
    {
        Application.Quit();
    }

    void Update()
    {

    }
}
