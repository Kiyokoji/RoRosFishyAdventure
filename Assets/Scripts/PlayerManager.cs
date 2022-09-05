using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView view;
    private Scene currentScene;
    private int sceneIndex;
    private Vector2 spawnPoint1;
    private Vector2 spawnPoint2;

    private bool player1;
    private bool player2;


    private void Awake()
    {
        view = GetComponent<PhotonView>();

        if (PhotonNetwork.IsMasterClient)
        {
            player1 = true;
            player2 = false;
        }
        else
        {
            player1 = false;
            player2 = true;
        }

        
    }

    void Start()
    {
        if (view.IsMine)
        {
            CreateController();
        }
    }

    void CreateController()
    {
        currentScene = SceneManager.GetActiveScene();

        switch (currentScene.buildIndex)
        {
            case 3:
                //Tutorial1
                spawnPoint1 = new Vector2(-192f, -2.5f);
                spawnPoint2 = new Vector2(-195f, -2.5f);
                break;
            case 4:
                //Tutorial2
                spawnPoint1 = new Vector2(-193, -2.5f);
                spawnPoint2 = new Vector2(-189, -2.5f);
                break;
            case 5:
                //Tutorial3
                spawnPoint1 = new Vector2(-196, -2.5f);
                spawnPoint2 = new Vector2(-192, -2.5f);
                break;
            case 6:
                //Tutorial4
                spawnPoint1 = new Vector2(-185, -2.5f);
                spawnPoint2 = new Vector2(-181, -2.5f);
                break;
            case 7:
                //LevelKai1
                spawnPoint1 = new Vector2(-182, -2.6f);
                spawnPoint2 = new Vector2(-178, -2.6f);
                
                break;
            case 8:
                //LevelAnton1
                spawnPoint1 = new Vector2(-218, -2.5f);
                spawnPoint2 = new Vector2(-214, -2.5f);
                break;
            case 9:
                //LevelKai2
                spawnPoint1 = new Vector2(-207, -2.5f);
                spawnPoint2 = new Vector2(-203, -2.5f);
                break;
            case 10:
                //LevelAnton2
                spawnPoint1 = new Vector2(-211, -2.5f);
                spawnPoint2 = new Vector2(-208, -2.5f);
                break;
        }

        if (player1)
        {
            PhotonNetwork.Instantiate("Harold", spawnPoint1, Quaternion.identity);
        } else if (player2)
        {
            PhotonNetwork.Instantiate("Harold2", spawnPoint2, Quaternion.identity);
        }
        
    }
}
