using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyRotation : MonoBehaviour
{

    public Transform sky;
    public float speed = 0.5f;
    
    
    void Start()
    {
        
    }


    void Update()
    {
        sky.Rotate(0,0,speed * Time.deltaTime);
    }
}
