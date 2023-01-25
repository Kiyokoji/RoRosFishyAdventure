using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrateDisable : MonoBehaviour
{
    public bool onScale;
    public ScaleMove scaleMove;
    
    private void OnDisable()
    {
        if (onScale)
        {
            if (scaleMove.isLeftScale)
            {
                scaleMove.scale.leftObjects--;
            }
            else
            {
                scaleMove.scale.rightObjects--;
            }
        }
    }

    public void SetScaleMove(ScaleMove scale)
    {
        scaleMove = scale;
    }
    
    public void ResetScaleMove()
    {
        scaleMove = null;
    }
}
