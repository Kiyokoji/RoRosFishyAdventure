using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Winch : MonoBehaviour
{
    public WinchMove winch;
    
    [HideInInspector] public bool goingUp;
    [HideInInspector] public bool goingDown;
    [HideInInspector] public bool isPressing;
    
    public float winchSpeed = 5f;

    void FixedUpdate()
    {
        if (isPressing)
        {
            if (goingDown && !goingUp)
            {
                winch.GoDown();
            }
            else if (goingUp && !goingDown)
            {
                winch.GoUp();
            }
        }
    }
}
