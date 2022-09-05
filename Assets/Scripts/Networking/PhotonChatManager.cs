using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Chat;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class PhotonChatManager : MonoBehaviour, IChatClientListener
{
    private ChatClient chatClient;
    public TextMeshProUGUI connectionState;
    public TMP_InputField messageInput;
    public TextMeshProUGUI messageArea;

    private string worldchat;

    private bool connected = false;


    void Start()
    {
        Application.runInBackground = true;

        //check if AppID is available
        if (string.IsNullOrEmpty(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat))
        {
            Debug.LogError("No AppID Provided");
            return;
        }
        worldchat = "world";

        //connect to chat on start
        GetConnected();
    }

    void Update()
    {
        if (chatClient != null)
        {
            this.chatClient.Service();
        }
    }

    public void GetConnected()
    {
        if (!connected)
        {
            chatClient = new ChatClient(this);
            chatClient.ChatRegion = "eu";
            chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new Photon.Chat.AuthenticationValues(PhotonNetwork.NickName));
            connectionState.text = "Connecting...";
        }
    }

    public void OnConnected()
    {
        connected = true;

        chatClient.Subscribe(new string[] { worldchat });
        chatClient.SetOnlineStatus(ChatUserStatus.Online);
    }

    public void SendMsg()
    {
        chatClient.PublishMessage(worldchat, messageInput.text);
    }

    public void GetDisconnected()
    {
        chatClient.Disconnect(ChatDisconnectCause.None);
    }

    public void OnDisconnected()
    {
        connected = false;

        chatClient.Unsubscribe(new string[] { worldchat });
        chatClient.SetOnlineStatus(ChatUserStatus.Offline);
    }


    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        
        for (int i = 0; i < senders.Length; i++)
        {
            messageArea.text += "[" + System.DateTime.Now.ToString("hh:mm:ss") + "] " + senders[i] + ": " + messages[i] + "\n";
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {

    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        foreach (var channel in channels)
        {
            this.chatClient.PublishMessage(channel, "joined");
        }

        connectionState.text = "Connected";
    }

    public void OnUnsubscribed(string[] channels)
    {
        messageArea.text = "";
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {

    }

    public void OnUserSubscribed(string channel, string user)
    {

    }

    public void OnUserUnsubscribed(string channel, string user)
    {

    }

    public void DebugReturn(DebugLevel level, string message)
    {
    }

    public void OnChatStateChange(ChatState state)
    {
    }
}