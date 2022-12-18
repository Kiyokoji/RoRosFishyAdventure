using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleMove : MonoBehaviour
{
    public Scale scale;
    
    public Transform _up;
    public Transform _down;
    public Transform _default;

    public bool isLeftScale;
    
    private float _speed;

    private void Awake()
    {
        _speed = scale.speed;
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

    public void GoDefault()
    {
        this.transform.position = 
            Vector2.MoveTowards(
                this.transform.position, 
                _default.position, 
                _speed * Time.fixedDeltaTime
            );
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player") || col.CompareTag("Object"))
        {
            if (isLeftScale)
            {
                scale.leftTrigger = true;
            }
            else
            {
                scale.rightTrigger = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") || col.CompareTag("Object"))
        {
            if (isLeftScale)
            {
                scale.leftObjects++;
            }
            else
            {
                scale.rightObjects++;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player") || col.CompareTag("Object"))
        {
            if (isLeftScale)
            {
                scale.leftObjects--;
                scale.leftTrigger = false;
            }
            else
            {
                scale.rightObjects--;
                scale.rightTrigger = false;
            }
        }
    }
}
