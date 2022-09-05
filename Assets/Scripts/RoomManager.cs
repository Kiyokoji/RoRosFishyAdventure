using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;
    public bool player1;
    public bool player2;

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

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        switch (scene.buildIndex)
        {
            case 0:
                MusicPlayer.instance.PlayMenuMusic();
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                //Tutorial1
                FindObjectOfType<AudioManager>().Play("EnterGame");
                PhotonNetwork.Instantiate("PlayerManager", Vector3.zero, Quaternion.identity);
                MusicPlayer.instance.PlayFloodedMusic();
                break;
            case 4:
                //Tutorial2
                PhotonNetwork.Instantiate("PlayerManager", Vector3.zero, Quaternion.identity);
                MusicPlayer.instance.PlayGameMusic();
                break;
            case 5:
                //Tutorial3
                PhotonNetwork.Instantiate("PlayerManager", Vector3.zero, Quaternion.identity);
                MusicPlayer.instance.PlayFloodedMusic();
                break;
            case 6:
                //Tutorial4
                PhotonNetwork.Instantiate("PlayerManager", Vector3.zero, Quaternion.identity);
                MusicPlayer.instance.PlayGameMusic();
                break;
            case 7:
                //LevelKai1
                PhotonNetwork.Instantiate("PlayerManager", Vector3.zero, Quaternion.identity);
                MusicPlayer.instance.PlayGameMusic();
                break;
            case 8:
                //LevelAnton1
                PhotonNetwork.Instantiate("PlayerManager", Vector3.zero, Quaternion.identity);
                MusicPlayer.instance.PlayFloodedMusic();
                break;
            case 9:
                //LevelKai2
                PhotonNetwork.Instantiate("PlayerManager", Vector3.zero, Quaternion.identity);
                MusicPlayer.instance.PlayGameMusic();
                break;
            case 10:
                //LevelAnton2
                PhotonNetwork.Instantiate("PlayerManager", Vector3.zero, Quaternion.identity);
                MusicPlayer.instance.PlayFloodedMusic();
                break;
            case 11:
                //Ending
                MusicPlayer.instance.PlayEndMusic();
                break;

        }
    }
}
