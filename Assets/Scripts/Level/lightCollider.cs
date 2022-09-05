using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class lightCollider : MonoBehaviour
{
    private bool flashLight = false;

    private void Update()
    {
        flashLight = GetComponentInParent<FlashLight>().FlashlightOn;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (flashLight && collision.CompareTag("Platform"))
        {
            collision.GetComponent<PhotonView>().RPC("EnablePlatform", RpcTarget.All);
            //Debug.Log("enter platform collision");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (flashLight && collision.CompareTag("Platform"))
        {
            collision.GetComponent<PhotonView>().RPC("DisablePlatform", RpcTarget.All);
            //Debug.Log("exit platform collision");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Platform"))
        {
            //Debug.Log("colliding");
        }
    }

    
}
