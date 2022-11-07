using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float bounceStrength = 20f;
    
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("collision");
        
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bounceStrength, ForceMode2D.Impulse);
        }
    }
}
