using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviourPun
{
    PhotonView view;

    public GameObject[] playerPrefab;
    public Vector3 SpawnPoint;

    void Start()
    {
        view = GetComponent<PhotonView>();

        if (PhotonNetwork.IsConnected && !PhotonNetwork.OfflineMode)
        {
            if (RoomManager.instance.player1)
            {
                PhotonNetwork.Instantiate("Harold", SpawnPoint, Quaternion.identity);
            }
            else if (RoomManager.instance.player2)
            {
                PhotonNetwork.Instantiate("Harold2", SpawnPoint, Quaternion.identity);
            }
        }

        
        /*

        if (PhotonNetwork.IsConnected && !PhotonNetwork.OfflineMode)
        {
            GameObject playerToSpawn = playerPrefab[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
            PhotonNetwork.Instantiate(playerToSpawn.name, SpawnPoint, Quaternion.identity);
        }
        */
    }
    /*

    [PunRPC]
    private void SpawnPlayer()
    {
        GameObject playerToSpawn = playerPrefab[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
        PhotonNetwork.Instantiate(playerToSpawn.name, SpawnPoint, Quaternion.identity);
    }

    private void DeleteAllPlayers()
    {
        player1 = GameObject.FindGameObjectsWithTag("Player1");
        player2 = GameObject.FindGameObjectsWithTag("Player2");

        for (var i = 0; i < player1.Length; i++)
        {
            Destroy(player1[i]);
        }

        for (var i = 0; i < player2.Length; i++)
        {
            Destroy(player2[i]);
        }
    }
    */
}
