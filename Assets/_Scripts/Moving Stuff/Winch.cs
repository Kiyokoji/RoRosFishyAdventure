using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Winch : MonoBehaviour
{
    public WinchMove winch;
    
    [HideInInspector] public bool triggerArea;

    private bool goingUp;
    private bool goingDown;
    private bool isPressing;

    public float speed = 5f;

    private PlayerInputActions controls;

    public PlayerController.PlayerController player;
    
    private void Awake()
    {
        controls = new PlayerInputActions();
        controls.Enable();
    }
    
    private void Update()
    {
        if (!triggerArea) return;

        isPressing = controls.Input.Left.IsPressed() || controls.Input.Right.IsPressed();
        goingDown = controls.Input.Left.IsPressed();
        goingUp   = controls.Input.Right.IsPressed();


        if (isPressing)
        {
            
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;

        if (isPressing)
        {
            if (goingDown && !goingUp)
            {
                player.CanMove = false;
                winch.GoDown();
            }
            else if (goingUp && !goingDown)
            {
                player.CanMove = false;
                winch.GoUp();
            }
            else
            {
                player.CanMove = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col) return;
        
        if (col.CompareTag("Player"))
        {
            player = col.gameObject.GetComponent<PlayerController.PlayerController>();
            triggerArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            player.CanMove = true;
            player = null;
            triggerArea = false;
        }
    }
    
}
