using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;


//swap player prefab once player 1 has joined 
//then it disables the joining function to prevent error messages whenever space is pressed

public class PlayerSpawn : MonoBehaviour
{
    public Object player1;
    public Object player2;
    
    private PlayerInputManager manager;

    private void Awake()
    {
        manager = GetComponent<PlayerInputManager>();
    }

    public void SwapPrefab()
    {
        //manager.playerPrefab.transform.position = spawn1.position;
        //spawn1 = spawn2;
        
        if(manager != null)
        manager.playerPrefab = (GameObject)player2;

        if (manager.playerCount > 1)
        {
            manager.DisableJoining();
        }
    }
}
