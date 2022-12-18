using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : MonoBehaviour
{
    public ScaleMove leftScale;
    public ScaleMove rightScale;

    [HideInInspector] public bool leftTrigger;
    [HideInInspector] public bool rightTrigger;
    
    [ReadOnly] public int leftObjects;
    [ReadOnly] public int rightObjects;

    public float speed = 5f;

    private void FixedUpdate()
    {
        if ((leftTrigger && rightTrigger) && (leftObjects == rightObjects))
        {
            rightScale.GoDefault();
            leftScale.GoDefault();
        } 
        else if (!leftTrigger && !rightTrigger)
        {
            rightScale.GoDefault();
            leftScale.GoDefault();
        }
        else if ((leftTrigger && !rightTrigger) || (leftObjects > rightObjects))
        {
            leftScale.GoDown();
            rightScale.GoUp();
        }
        else if ((rightTrigger && !leftTrigger) || (leftObjects < rightObjects))
        {
            rightScale.GoDown();
            leftScale.GoUp();
        }
        
    }
    
}

public class ReadOnlyAttribute : Attribute
{
    
}
