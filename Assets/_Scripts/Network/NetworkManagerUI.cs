using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Multiplayer.Tools.NetStatsMonitor;
using Unity.Netcode;


public class NetworkManagerUI : MonoBehaviour
{
    private Ping ping;
    
    [SerializeField] private Button serverButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    [SerializeField] private TextMeshProUGUI playersInGameText;
    [SerializeField] private TextMeshProUGUI playerPing;

    public PlayerManager playerManager;

    private void Awake()
    {
       
        
        ping = new Ping("ipaddress");
        
        Cursor.visible = true;
        
        serverButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
        });
        
        clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
        
        hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });
    }

    private void Update()
    {

        //playerPing.text = $"Ping: {NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetCurrentRtt(playerManager.OwnerClientId).ToString()}";
        
        playersInGameText.text = $"Player count: {playerManager.PlayersInGame}";
    }
}
