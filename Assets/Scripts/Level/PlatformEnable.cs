using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlatformEnable : MonoBehaviour
{

    private void Start()
    {

    }

    [PunRPC]
    public void EnablePlatform()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Ground");
    }

    [PunRPC]
    public void DisablePlatform()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Platform");
    }

    public void Completed()
    {

    }
}
