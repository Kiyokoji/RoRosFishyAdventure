using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

//swap player prefab once player 1 has joined 
//then it disables the joining function to prevent error messages whenever space is pressed

public class PlayerSpawn : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    
    public GameObject player1;
    public GameObject player2;
    
    private PlayerInputManager manager;

    private void OnDisable()
    {
        manager.playerPrefab = player1;
    }

    private void Awake()
    {
        manager = GetComponent<PlayerInputManager>();

        //set player prefab to player 1
        manager.playerPrefab = player1;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Join.performed += JoinPlayerFromAction;
    }

    private void JoinPlayerFromAction(InputAction.CallbackContext ctx)
    {
        if (SceneManager.GetActiveScene().name == "StarMenu") return;
        if (manager.playerCount > 1) return;

        //spawn player
        manager.JoinPlayerFromActionIfNotAlreadyJoined(ctx);
    }

    public void SwapPrefab()
    {
        manager.playerPrefab = player2;

        if (manager.playerCount == 2)
        {
            //manager.playerPrefab = player1;
        }
        else
        {
            //manager.playerPrefab = player2;
        }

        if (manager.playerCount > 1)
        {
            manager.DisableJoining();
        }
    }
}
