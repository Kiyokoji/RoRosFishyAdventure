using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTrigger : MonoBehaviour
{
    private JumpSwitch jumpSwitch;

    private void Awake()
    {
        jumpSwitch = GetComponentInParent<JumpSwitch>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (jumpSwitch.weighted)
        {
            if(col.CompareTag("Player1") || col.CompareTag("Player2") || col.CompareTag("Object"))
            {
                jumpSwitch.Activate();
            }
        }
        else 
        {
            if (!jumpSwitch.toggled && (col.CompareTag("Player1") || col.CompareTag("Player2") || col.CompareTag("Object")))
            {
                jumpSwitch.Activate();
                jumpSwitch.toggled = true;
            }
            else if (jumpSwitch.toggled && (col.CompareTag("Player1") || col.CompareTag("Player2") || col.CompareTag("Object")))
            {
                jumpSwitch.Deactivate();
                jumpSwitch.toggled = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (jumpSwitch.weighted)
        {
            jumpSwitch.Deactivate();
        }
    }
}
