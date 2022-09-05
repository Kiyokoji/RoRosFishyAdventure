using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FriedFish : MonoBehaviour
{
    public Transform spawnPoint;
    private PhotonView view;

    private GameObject player;

    void Start()
    {
        view = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player1") || collision.CompareTag("Player2"))
        {
            FindObjectOfType<AudioManager>().Play("Death");
            FindObjectOfType<AudioManager>().Play("Death2");
            player = collision.gameObject;
            view.RPC("TeleportPlayer", RpcTarget.All);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        player = null;
    }

    [PunRPC]
    public void TeleportPlayer()
    {
        player.gameObject.transform.position = spawnPoint.position;
    }
}
