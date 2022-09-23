using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    public static NetworkPlayer Local { get; set; }   //access local player anywhere in the code

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;  //set this to the local player

            Debug.Log("Spawned local player");
        }
        else Debug.Log("Spawned remote player");
    }

    public void PlayerLeft(PlayerRef player)
    {
        //check if I have authority over player
        if (player == Object.InputAuthority)
            Runner.Despawn(Object);
    }
}