using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RopeRenderer : MonoBehaviour
{
    private LineRenderer rope;
    public Transform start;
    public Transform target;
    public Transform parent;

    private void Awake()
    {
        rope = GetComponent<LineRenderer>();
    }

    void Update()
    {
        //if (GUI.changed) EditorUtility.SetDirty (this);
        
        rope.SetPosition(0, start.position - parent.position);
        rope.SetPosition(1, target.position - parent.position);
    }
}
