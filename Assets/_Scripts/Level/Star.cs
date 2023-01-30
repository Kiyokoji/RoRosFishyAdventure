using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public bool placed;
    public int count;
    public StarCluster cluster;

    private void Awake()
    {
        placed = false;
    }
}
