using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OnlineManager : MonoBehaviour
{
    [Tooltip("Toggle Networking")]
    public bool Online;

    [Tooltip("Game objects for testing")]
    public List<GameObject> testObject;

    //private GameObject chatWindow;

    private void Awake()
    {
        //chatWindow = GameObject.Find("Canvas/ChatPanel");
    }

    void Start()
    {
        if (Online)
        {
            foreach (GameObject thisObject in testObject)
            {
                thisObject.SetActive(false);
            }
        }

        if (!Online)
        {
            PhotonNetwork.OfflineMode = true;
            PhotonNetwork.CreateRoom("some name");
            //chatWindow.SetActive(false);
            //GameManager.instance.chatActive = false;
            //Vector2 newSpawn = new Vector2(0, 0);
            //PhotonNetwork.Instantiate(playerPrefab.name, newSpawn, Quaternion.identity);
        }
    }

}
