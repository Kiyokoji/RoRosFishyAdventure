using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeRenderer : MonoBehaviour
{
    private LineRenderer rope;
    public Transform start;
    public Transform target;
    public Transform scale;

    private void Awake()
    {
        rope = GetComponent<LineRenderer>();
    }

    void Update()
    {
        rope.SetPosition(0, start.position - scale.position);
        rope.SetPosition(1, target.position - scale.position);
    }
}
