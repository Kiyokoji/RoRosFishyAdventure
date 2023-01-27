using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class Winch : MonoBehaviour
{
    public WinchMove winch;
    public EventReference wheelEventSound;
    [HideInInspector] public EventInstance wheelSound;

    [HideInInspector] public bool goingUp;
    [HideInInspector] public bool goingDown;
    [HideInInspector] public bool isPressing;
    
    public float winchSpeed = 5f;
    public bool isPlaying = false;
    
    
    private void Awake()
    {
        wheelSound = FMODUnity.RuntimeManager.CreateInstance(wheelEventSound);
    }

    void FixedUpdate()
    {
        if (isPressing)
        {
            if (!isPlaying)
            {
                wheelSound.start();
                isPlaying = true;
            }

            if (goingDown && !goingUp)
            {
                winch.GoDown();
            }
            else if (goingUp && !goingDown)
            {
                winch.GoUp();
            }
        } 
        else
        {
            wheelSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            isPlaying = false;
        }
    }
}
