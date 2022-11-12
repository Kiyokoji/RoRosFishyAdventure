using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public FMODUnity.StudioEventEmitter emitter;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Play Sound");

            emitter.EventInstance.setParameterByNameWithLabel("Parameter 1", "1");
        }
        
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Play Sound");

            emitter.EventInstance.setParameterByNameWithLabel("Parameter 1", "0");
        }
    }
}
