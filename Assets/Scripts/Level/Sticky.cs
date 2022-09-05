using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Sticky : MonoBehaviour
{
    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player1") || collision.CompareTag("Player2") || collision.CompareTag("Crate"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1"))
        {
            view.RPC("UnParentPlayer1", RpcTarget.All);
        }
        if (collision.CompareTag("Player2"))
        {
            view.RPC("UnParentPlayer2", RpcTarget.All);
        }
        if (collision.CompareTag("Crate"))
        {
            view.RPC("UnParentCrate", RpcTarget.All);
        }
    }

    [PunRPC]
    public void UnParentAll()
    {
        foreach (Transform child in transform)
        {
            child.transform.SetParent(null);
        }
    }

    [PunRPC]
    public void UnParentCrate()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Crate"))
            {
                child.transform.SetParent(null);
            }
        }
    }

    [PunRPC]
    public void UnParentPlayer1()
    {
        foreach(Transform child in transform)
        {
            if (child.CompareTag("Player1"))
            {
                child.transform.SetParent(null);
            }
        }
    }

    [PunRPC]
    public void UnParentPlayer2()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Player2"))
            {
                child.transform.SetParent(null);
            }
        }
    }

}
