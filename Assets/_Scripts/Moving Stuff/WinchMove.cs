using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class WinchMove : MonoBehaviour
{
    public Winch winch;
    
    public Transform _up;
    public Transform _down;
    public Transform wheel;

    private EventReference wheelSound;
    private float _speed;

    private void Awake()
    {
        wheelSound = winch.wheelSound;
        _speed = winch.winchSpeed;
    }

    public void GoUp()
    {
        if (transform.position == _up.position) return;
        
        FMODUnity.RuntimeManager.PlayOneShot(wheelSound);
        
        wheel.transform.Rotate(0f, 0f, -_speed);
        
        this.transform.position = 
            Vector2.MoveTowards(
                this.transform.position, 
                _up.position, 
                _speed * Time.fixedDeltaTime
            );
    }
    
    public void GoDown()
    {
        if (transform.position == _down.position) return;
        
        wheel.transform.Rotate(0f, 0f, _speed);
        
        this.transform.position = 
            Vector2.MoveTowards(
                this.transform.position, 
                _down.position, 
                _speed * Time.fixedDeltaTime
            );
    }
}
