using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class CrateSound : MonoBehaviour
{
    public EventReference crateSound;

    private int frameCounter = 0;
    public int framesToCount;
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        frameCounter++;
        
        if (col.collider.CompareTag("Ground"))
        {
            if (frameCounter % framesToCount == 0)
            {
                FMODUnity.RuntimeManager.PlayOneShot(crateSound);
            }

        }
    }
}
