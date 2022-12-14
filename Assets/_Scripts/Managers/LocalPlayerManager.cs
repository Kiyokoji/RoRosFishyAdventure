using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class LocalPlayerManager : MonoBehaviour
{
    private List<PlayerInput> players = new List<PlayerInput>();
    
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private List<LayerMask> playerLayers;
    public GameObject player1prefab, player2prefab;

    private InputDevice pairWithDevice;

    [HideInInspector] public PlayerInputManager playerInputManager;

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    private void Start()
    {
        //playerInputManager.playerPrefab = player1;
    }

    private void Update()
    {
        foreach (PlayerInput pInput in players)
        {
            Debug.Log(pInput.currentControlScheme);
        }
    }

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += AddPlayer;
        playerInputManager.onPlayerJoined += SwapPlayer;
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= AddPlayer;
        playerInputManager.onPlayerJoined -= SwapPlayer;
    }

    public void SwapPlayer(PlayerInput player)
    {
        if (players.Count == 0)
        {
            //playerInputManager.playerPrefab = player1;
        }
        else
        {
            //playerInputManager.playerPrefab = player2;
        }

    }

    public void SpawnPlayer1()
    {
        playerInputManager.playerPrefab = player1prefab;
        
        PlayerInput player1 = playerInputManager.JoinPlayer(0 ,-1,"", pairWithDevice = null);

        player1.transform.position = spawnPoints[0].transform.position;
        
        int layerToAdd = (int)Mathf.Log(playerLayers[players.Count - 1].value, 2);
        
        //set the layer
        player1.GetComponentInChildren<CinemachineVirtualCamera>().gameObject.layer = layerToAdd;
        
        //add the layer
        player1.GetComponentInChildren<Camera>().cullingMask |= 1 << layerToAdd;
    }
    
    public void SpawnPlayer2()
    {
        playerInputManager.playerPrefab = player2prefab;
        
        PlayerInput player2 = playerInputManager.JoinPlayer(1 ,-1,"", pairWithDevice = null);

        player2.transform.position = spawnPoints[1].transform.position;
        
        int layerToAdd = (int)Mathf.Log(playerLayers[players.Count - 1].value, 2);
        
        //set the layer
        player2.GetComponentInChildren<CinemachineVirtualCamera>().gameObject.layer = layerToAdd;
        
        //add the layer
        player2.GetComponentInChildren<Camera>().cullingMask |= 1 << layerToAdd;
    }
    
    
    public void AddPlayer(PlayerInput player)
    {
        players.Add(player);
        //playerInputManager.playerPrefab = player2;
        
        //convert layer mask to integer
        int layerToAdd = (int)Mathf.Log(playerLayers[players.Count - 1].value, 2);
        
        //set the layer
        player.GetComponentInChildren<CinemachineVirtualCamera>().gameObject.layer = layerToAdd;
        
        //add the layer
        player.GetComponentInChildren<Camera>().cullingMask |= 1 << layerToAdd;

        //set position 
        //player.transform.position = spawnPoints[players.Count].transform.position;
        
        

        //set the action in the custom cinemachine Input handler
        //player.GetComponentInChildren<InputHandler>()


    }
}
