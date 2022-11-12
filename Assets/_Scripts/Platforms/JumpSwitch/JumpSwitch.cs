using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Platforms{}

public class JumpSwitch : MonoBehaviour
{
    [SerializeField] private List<Activatable> activatableList;
    
    private bool active;
    [SerializeField] private float speed = 2f;

    public bool weighted = true;
    [HideInInspector] public bool toggled;
    
    [SerializeField] private GameObject waypoint1;
    [SerializeField] private GameObject waypoint2;
    
    [SerializeField] private Transform platform;
    
    private void FixedUpdate()
    {
        if (active)
        {
            platform.position = Vector2.MoveTowards(platform.position, waypoint2.transform.position, speed * Time.deltaTime);
        }
        else
        {
            platform.position = Vector2.MoveTowards(platform.position, waypoint1.transform.position, speed * Time.deltaTime);
        }
    }

    public void Activate()
    {
        active = true;
        
        foreach (var a in activatableList)
        {
            a.isActive = true;
        }
    }

    public void Deactivate()
    {
        active = false;
        
        foreach (var a in activatableList)
        {
            a.isActive = false;
        }
    }
}
