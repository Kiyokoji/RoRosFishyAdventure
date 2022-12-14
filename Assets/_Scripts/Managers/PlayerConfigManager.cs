using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.SceneManagement;

public class PlayerConfigManager : MonoBehaviour
{
    public static PlayerConfigManager Instance { get; private set; }
    
    private List<PlayerConfiguration> playerConfigs;

    private PlayerInputManager manager;

    [SerializeField] private int MaxPlayers = 2;

    public GameObject player1Menu;
    public GameObject player2Menu;

    public GameObject player1Prefab;
    public GameObject player2Prefab;

    [HideInInspector] public bool player1Ready;
    [HideInInspector] public bool player2Ready;

    private void Awake()
    {
        manager = GetComponent<PlayerInputManager>();

        if (Instance != null) { /**/ } 
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            playerConfigs = new List<PlayerConfiguration>();
        }
    }

    private void Update()
    {

    }

    public void ReadyPlayer(int index)
    {
        if (index == 1)
        {
            player1Ready = true;
        } else if (index == 2)
        {
            player2Ready = true;
        }
        
        if (player1Ready && player2Ready)
        {
            Debug.Log("Loading Scene");
            SceneManager.LoadScene("Game");
        }
        
    }

    public void HandlePlayerJoin(PlayerInput player)
    {
        if (player.playerIndex == 0)
        {
            manager.playerPrefab = player1Prefab;
            player1Menu.SetActive(false);
            manager.playerPrefab = player2Prefab;
        } else if (player.playerIndex == 1)
        {
            player2Menu.SetActive(false);
        }

        Debug.Log("Player joined: " + player.playerIndex);
        player.transform.SetParent(transform);
        
        //make sure the player is not already added
        if (playerConfigs.Any(p => p.PlayerIndex == player.playerIndex))
        {
            playerConfigs.Add(new PlayerConfiguration(player));
        }
    }

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return playerConfigs;
    }
}

public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput input)
    {
        PlayerIndex = input.playerIndex;
        Input = input;
    }
    
    public PlayerInput Input { get; set; }
    
    public int PlayerIndex { get; set; }
    
    public bool IsReady { get; set; }
}
