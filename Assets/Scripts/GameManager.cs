using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector] public bool chatActive = false;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    [PunRPC]
    public void Restart()
    {
        PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().name);
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            PhotonNetwork.AutomaticallySyncScene = false;
            PhotonNetwork.Disconnect();
        }


        /*

        if (Input.GetKeyDown(KeyCode.P)) 
        {
            pingWindow.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("Tutorial1");
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("Tutorial2");
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("Tutorial3");
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("Tutorial4");
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("LevelAnton1");
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("LevelAnton2");
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("LevelKai1");
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("LevelKai2");
            }
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            SceneManager.LoadScene("Ending");
        }
        */
    }
}
