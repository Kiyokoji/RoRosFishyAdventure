using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinchMove : MonoBehaviour
{
    public Winch winch;
    
    public Transform _up;
    public Transform _down;
    
    private float _speed;

    private void Awake()
    {
        _speed = winch.speed;
    }

    public void GoUp()
    {
        this.transform.position = 
            Vector2.MoveTowards(
                this.transform.position, 
                _up.position, 
                _speed * Time.fixedDeltaTime
            );
    }
    
    public void GoDown()
    {
        this.transform.position = 
            Vector2.MoveTowards(
                this.transform.position, 
                _down.position, 
                _speed * Time.fixedDeltaTime
            );
    }
}
