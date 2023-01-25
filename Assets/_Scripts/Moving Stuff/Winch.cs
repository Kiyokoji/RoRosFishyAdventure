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
    public EventReference wheelSound;

    private EventInstance audioEvent;
    
    [HideInInspector] public bool goingUp;
    [HideInInspector] public bool goingDown;
    [HideInInspector] public bool isPressing;
    
    public float winchSpeed = 5f;

    private void Awake()
    {
        
        
    }

    void FixedUpdate()
    {
        if (isPressing)
        {
            //audioEvent = RuntimeManager.CreateInstance(wheelSound);
            //FMOD.Studio.PLAYBACK_STATE playbackState;
            //audioEvent.getPlaybackState(out playbackState);
            //bool isPLaying = playbackState != FMOD.Studio.PLAYBACK_STATE.STOPPED;
            
            //if (!isPLaying)
            //{
            //    audioEvent.start();
            //    
            //    Debug.Log("callign sound");
            //}
            //else
            //
             //   audioEvent.stop(STOP_MODE.IMMEDIATE);
            //}

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
