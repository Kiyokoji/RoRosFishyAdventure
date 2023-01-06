using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class CrateSound : MonoBehaviour
{
    public EventReference crateSound;
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Ground"))
        {
            FMODUnity.RuntimeManager.PlayOneShot(crateSound);
        }
    }
}
