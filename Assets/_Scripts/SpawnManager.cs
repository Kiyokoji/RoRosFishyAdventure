using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform spawnPoint1;
    public Transform spawnPoint2;

    private GameObject player1;
    private GameObject player2;
    
    public void RespawnPlayer1()
    {
        player1 = GameObject.FindGameObjectWithTag("Player1");
        
        if(player1 != null)
        player1.transform.position = spawnPoint1.position;
    }
    
    public void RespawnPlayer2()
    {
        player2 = GameObject.FindGameObjectWithTag("Player2");
        
        if(player2 != null)
        player2.transform.position = spawnPoint2.position;
    }

}
