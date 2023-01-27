using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


//swap player prefab once player 1 has joined 
//then it disables the joining function to prevent error messages whenever space is pressed

public class PlayerSpawn : MonoBehaviour
{
    public Object player1;
    public Object player2;

    public Transform spawn1;
    public Transform spawn2;

    public PlayerInputManager manager;

    public void SwapPrefab()
    {
        //manager.playerPrefab.transform.position = spawn1.position;
        //spawn1 = spawn2;
        
        manager.playerPrefab = (GameObject)player2;

        if (manager.playerCount > 1)
        {
            manager.DisableJoining();
        }
    }
}
