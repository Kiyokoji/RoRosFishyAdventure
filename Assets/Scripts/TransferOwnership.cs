using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferOwnership : MonoBehaviourPun
{
    public void RequestOwnership()
    {
        base.photonView.RequestOwnership();
    }
}
