using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkPlayer : MonoBehaviourPunCallbacks, IPunObservable
{
    PhotonView view;

    private Vector3 correctPlayerPos;
    private Quaternion correctPlayerRot;

    void Start()
    {
        view = GetComponent<PhotonView>();
    }

    void FixedUpdate()
    {
        if (!view.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.fixedDeltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.fixedDeltaTime);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else if (stream.IsReading)
        {
            this.correctPlayerPos = (Vector3)stream.ReceiveNext();
            this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
        }
    }
}