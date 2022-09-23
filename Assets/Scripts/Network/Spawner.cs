using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.PlayerLoop;

public class Spawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkPrefabRef playerPrefab;

    private SceneRef _currentScene;
    private bool _currentSceneOutdated;

    private Dictionary<PlayerRef, NetworkObject> spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    NetworkRunner _runner;
    CharacterInputHandler characterInputHandler;

    private void Start()
    {
        DebugScene(_currentSceneOutdated);
    }


    protected virtual void LateUpdate()
    {
        if (!_runner) return;

        if (_runner.CurrentScene != _currentScene)
        {
            _currentSceneOutdated = true;
        }
    }


    async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            Debug.Log("OnPlayerJoined we are server. Spawning Player");

            NetworkObject networkPlayerObject = runner.Spawn(playerPrefab, Utils.GetRandomSpawnPoint(), Quaternion.identity, player);

            spawnedCharacters.Add(player, networkPlayerObject);
        }

    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            spawnedCharacters.Remove(player);
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (characterInputHandler == null && NetworkPlayer.Local != null)
            characterInputHandler = NetworkPlayer.Local.GetComponent<CharacterInputHandler>();

        if (characterInputHandler != null)
        {
            input.Set(characterInputHandler.GetNetworkInput());
        }
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        //Debug.Log("OnConnectedToServer");

    }

    private void OnGUI()
    {
        if (_runner == null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            {
                StartGame(GameMode.Host);
            }
            if (GUI.Button(new Rect(0, 45, 200, 40), "Join"))
            {
                StartGame(GameMode.Client);
            }
            if (GUI.Button(new Rect(0, 90, 200, 40), "Single"))
            {
                StartGame(GameMode.Single);
            }
        }
    }


    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        Debug.Log("Input Missing");
    }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log("Shutdown");
    }
    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        Debug.Log("Disconnected from server");
    }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        Debug.Log("Connection request");
    }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.Log("Connection Failed");
    }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        throw new NotImplementedException();
    }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        Debug.Log("Session list update");
    }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        throw new NotImplementedException();
    }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        Debug.Log("Host Migration");
    }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        throw new NotImplementedException();
    }
    public void OnSceneLoadDone(NetworkRunner runner)
    {
        Debug.Log("Scene load started");
    }
    public void OnSceneLoadStart(NetworkRunner runner)
    {
        Debug.Log("Scene load done");
    }


    void DebugScene(bool yeet)
    {

    }
}
