using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeLevel : MonoBehaviour
{
    [SerializeField] private Transform player1Spawn;
    [SerializeField] private Transform player2Spawn;
    [SerializeField] private GameObject player1Prefab;
    [SerializeField] private GameObject player2Prefab;

    private void Start()
    {
        Instantiate(player1Prefab, player1Spawn);
        Instantiate(player2Prefab, player2Spawn);


        /*
        
        var playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs().ToArray();

        for (int i = 0; i < playerConfigs.Length; i++)
        {
            var player = Instantiate(playerPrefab[i], playerSpawns[i].position, playerSpawns[i].rotation, gameObject.transform);
        }
        
        */
    }
}
