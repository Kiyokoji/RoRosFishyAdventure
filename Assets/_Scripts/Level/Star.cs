using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public Star nextStar;
    public bool placed;

    private void Awake()
    {
        placed = false;
    }
}
