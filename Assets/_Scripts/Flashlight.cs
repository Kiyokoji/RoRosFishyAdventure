using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private Light2D light2D;
    private bool isFlashlightActive = false;

    [SerializeField] private GameObject lightGameObject;
    
    private PlayerInputActions playerInputActions;

    // Start is called before the first frame update
    void Start()
    {
        light2D = GetComponent<Light2D>();
    }

    private void OnEnable()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Flashlight.performed += Flashlight_performed;
    }
    
    private void OnDisable()
    {
        playerInputActions.Player.Disable();
    }

    private void Flashlight_performed(InputAction.CallbackContext ctx)
    {
        ToggleFlashlight();
    }

    private void ToggleFlashlight()
    {
        isFlashlightActive = !isFlashlightActive;

        if (isFlashlightActive)
        {
            light2D.gameObject.SetActive(true);
        }
        else
        {
            light2D.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
