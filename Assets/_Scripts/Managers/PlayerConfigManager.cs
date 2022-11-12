using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.SceneManagement;

public class PlayerConfigManager : MonoBehaviour
{
    private List<InputDevice> devices = new List<InputDevice>();
    
    public static PlayerConfigManager Instance { get; private set; }
    
    private List<PlayerConfiguration> playerConfigs;

    [SerializeField] private int MaxPlayers = 2;

    private void Awake()
    {
        foreach (InputDevice device in InputSystem.devices)
        {
            Debug.Log(device.name);
            Debug.Log(device.description);
            devices.Add(device);
        }

        
        
        if (Instance != null) { /**/ } else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            playerConfigs = new List<PlayerConfiguration>();
        }
    }

    public void ReadyPlayer(int index)
    {
        playerConfigs[index].IsReady = true;

        //check to see if all players are joined and set to ready
        if (playerConfigs.Count == MaxPlayers && playerConfigs.All(p => p.IsReady == true))
        {
            SceneManager.LoadScene("Game");
        }
    }

    public void HandlePlayerJoin(PlayerInput player)
    {
        Debug.Log("Player joined: " + player.playerIndex);
        player.transform.SetParent(transform);
        
        //make sure the player is not already added
        if (playerConfigs.Any(p => p.PlayerIndex == player.playerIndex))
        {
            playerConfigs.Add(new PlayerConfiguration(player));
        }
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
