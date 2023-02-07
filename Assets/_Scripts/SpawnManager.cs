using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public Transform blankPoint;

    public GameObject player1;
    public GameObject player2;

    public void RespawnPlayer(int id, Transform player)
    {
        if (id == 1)
        {
            player.position = spawnPoint1.position;
            if (player1 == null) player1 = player.gameObject;
        }
        else if (id == 2)
        {
            player.position = spawnPoint2.position;
            if (player2 == null) player2 = player.gameObject;
        }
        else if (id == 3)
        {
            player.position = blankPoint.position;
        }

    }
}
