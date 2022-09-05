using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class StickyPlatform : MonoBehaviour
{
    PhotonView view;
    private void Start()
    {
        view = GetComponent<PhotonView>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Harold(Clone)" || collision.gameObject.name == "Harold")
        {
            GameObject Harold1 = collision.gameObject;
            Harold1.gameObject.transform.SetParent(transform);
        }
        else if (collision.gameObject.name == "Harold2(Clone)" || collision.gameObject.name == "Harold2")
        {
            GameObject Harold2 = collision.gameObject;
            Harold2.gameObject.transform.SetParent(transform);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Harold(Clone)" || collision.gameObject.name == "Harold")
        {
            GameObject Harold1 = collision.gameObject;
            Harold1.gameObject.transform.SetParent(null);
        }
        else if (collision.gameObject.name == "Harold2(Clone)" || collision.gameObject.name == "Harold2")
        {
            GameObject Harold2 = collision.gameObject;
            Harold2.gameObject.transform.SetParent(null);
        }
    }
}