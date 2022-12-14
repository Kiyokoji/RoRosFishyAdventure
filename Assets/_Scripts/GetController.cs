using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GetController : MonoBehaviour
{
    private PlayerInput input;

    [SerializeField] private GameObject gamepad;
    [SerializeField] private GameObject keyboard;

    private PlayerSetupMenuController setup;
    
    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        setup = GetComponent<PlayerSetupMenuController>();
        
        setup.SetPlayerIndex(input.playerIndex);
        
        if (input.currentControlScheme == "Gamepad")
        {
            gamepad.SetActive(true);
        } 
        else if(input.currentControlScheme == "Keyboard" || input.currentControlScheme == "Mouse")
        {
            keyboard.SetActive(true);
        }
    }
}
