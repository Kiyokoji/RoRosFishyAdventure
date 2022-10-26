using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    private NetworkVariable<int> playersInGame = new NetworkVariable<int>();

    public int PlayersInGame
    {
        get
        {
            return playersInGame.Value;
        }
    }

    void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if (NetworkManager.Singleton.IsServer)
            {
                playersInGame.Value++;
            }
        };
        
        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if (NetworkManager.Singleton.IsServer)
            {
                playersInGame.Value--;
            }
        };
        
    }
    
    void Update()
    {
        

    }
}
