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

    public void RespawnPlayer(int id, Transform player)
    {
        if (id == 1)
        {
            player.position = spawnPoint1.position;
        }
        else if (id == 2)
        {
            player.position = spawnPoint2.position;
        }

    }
}
