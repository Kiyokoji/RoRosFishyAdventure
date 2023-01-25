using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float bounceStrength = 20f;
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player1") || col.gameObject.CompareTag("Player2"))
        {
            var player = col.gameObject.GetComponent<PlayerController.PlayerController>();

            player._speed.y = bounceStrength;
        }
    }
}
