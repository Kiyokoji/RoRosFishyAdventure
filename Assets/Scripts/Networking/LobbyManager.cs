using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Player = Photon.Realtime.Player;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_Dropdown sceneList;

    public GameObject lobbyPanel;
    public GameObject roomPanel;
    public TMP_Text roomName;
    public TMP_InputField roomInputField;

    public RoomItem roomItemPrefab;
    List<RoomItem> roomItemsList = new List<RoomItem>();
    public Transform contentObject;

    public float timeBetweenUpdates = 0.5f;
    float nextUpdateTime;

    public List<PlayerItem> playerItemsList = new List<PlayerItem>();
    public PlayerItem playerItemPrefab;
    public Transform playerItemParent;

    public GameObject playButton;
    public Button createRoomButton;

    void Start()
    {
        Debug.ClearDeveloperConsole();
        PhotonNetwork.JoinLobby();
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            sceneList.gameObject.SetActive(true);
        }
        else
        {
            sceneList.gameObject.SetActive(false);
        }

        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            playButton.SetActive(true);
        } else
        {
            playButton.SetActive(false);
        }

        //Debug.Log(PhotonNetwork.NetworkClientState.ToString());
        //Debug.Log("Number of connected players: " + PhotonNetwork.PlayerListOthers.Length);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            createRoomButton.onClick.Invoke();
        }
    }

    public void OnClickPlayButton()
    {
        PhotonNetwork.LoadLevel(sceneList.captionText.text);
        Debug.Log("Loading: " + sceneList.captionText.text);
    }

    public void OnClickBackButton()
    {
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("MainMenu");
    }

    void UpdatePlayerList()
    {
        foreach (PlayerItem item in playerItemsList)
        {
            Destroy(item.gameObject);
        }
        playerItemsList.Clear();

        if(PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach (KeyValuePair<int, Photon.Realtime.Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerItemParent);
            newPlayerItem.SetPlayerInfo(player.Value);

            if (player.Value == PhotonNetwork.LocalPlayer)
            {
                newPlayerItem.ApplyLocalChanges();
            }

            playerItemsList.Add(newPlayerItem);
        }
    }

    public void OnClickCreate()
    {
        if (roomInputField.text.Length >= 1)
        {
            PhotonNetwork.CreateRoom(roomInputField.text, new RoomOptions() { MaxPlayers = 2, BroadcastPropsChangeToAll = true });
        }
    }

    public void OnClickJoin()
    {
        PhotonNetwork.JoinRoom(roomInputField.text);
    }

    public override void OnJoinedRoom()
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        roomName.text = PhotonNetwork.CurrentRoom.Name;

        UpdatePlayerList();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if(Time.time >= nextUpdateTime)
        {
            UpdateRoomList(roomList);
            nextUpdateTime = Time.time + timeBetweenUpdates;
        }
    }

    void UpdateRoomList(List<RoomInfo> list)
    {
        foreach (RoomItem item in roomItemsList)
        {
            Destroy(item.gameObject);
        }
        roomItemsList.Clear();

        foreach (RoomInfo room in list)
        {
            if(room.PlayerCount > 0)
            {
                if (room.PlayerCount != 2)
                {
                    RoomItem newRoom = Instantiate(roomItemPrefab, contentObject);
                    newRoom.SetRoomName(room.Name);
                    roomItemsList.Add(newRoom);
                }
            }
        }
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        //Find a way to destroy room is master client leaves
        //PhotonNetwork.DestroyAll();
    }

    public override void OnLeftRoom()
    {
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        UpdatePlayerList();
    }

}
