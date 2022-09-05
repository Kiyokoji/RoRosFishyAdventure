using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LagCompensation : MonoBehaviour, IPunObservable
{
    PhotonView photonView;

    Vector3 latestPos;
    Quaternion latestRot;

    float currentTime = 0;
    double currentPacketTime = 0;
    double lastPacketTime = 0;

    Vector3 positionAtLastPacket = Vector3.zero;
    Quaternion rotationAtLastPacket = Quaternion.identity;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        //PhotonNetwork.SendRateOnSerialize = 10;
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            //lag compensation
            double timeToReachGoal = currentPacketTime - lastPacketTime;
            currentTime += Time.deltaTime;

            //update remote player
            transform.position = Vector3.Lerp(positionAtLastPacket, latestPos, (float)(currentTime / timeToReachGoal));
            transform.rotation = Quaternion.Lerp(rotationAtLastPacket, latestRot, (float)(currentTime / timeToReachGoal));
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //we own this player, send others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            //network player, receive data
            latestPos = (Vector3)stream.ReceiveNext();
            latestRot = (Quaternion)stream.ReceiveNext();

            //Lag compensation
            currentTime = 0.0f;
            lastPacketTime = currentPacketTime;
            currentPacketTime = info.SentServerTime;
            positionAtLastPacket = transform.position;
            rotationAtLastPacket = transform.rotation;
        }
    }

}
