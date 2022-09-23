using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public struct NetworkInputData : INetworkInput
{
    public Vector2 movementInput;

    public Vector3 mousePos;

    public NetworkBool getJumpDown;  
    public NetworkBool getJumpUp;  
    public NetworkBool getJumpHold;

    public NetworkBool getFlashlightToggle;

}
