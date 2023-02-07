using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Mathematics;
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

    [ReadOnly] public GameObject blank;

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

        if (manager.playerCount == 1 && !FindObjectOfType<SpawnManager>().player1.GetComponent<PlayerController.PlayerController>().Grounded) return;
        
        //spawn player
        manager.JoinPlayerFromActionIfNotAlreadyJoined(ctx);

        if (manager.playerCount == 2 && blank == null)
        {
            blank = Instantiate((GameObject)player2);
            blank.GetComponent<PlayerController.PlayerController>().SetBlank();
            blank.GetComponent<PlayerController.PlayerController>().playerID = 3;
            //blank.transform.position = new Vector3(-100, 0);
            //blank.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
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
