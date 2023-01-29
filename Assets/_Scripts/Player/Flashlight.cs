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

    [SerializeField] private PolygonCollider2D flashCollider;
    
    private PlayerInputActions playerInputActions;
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
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
            flashCollider.enabled = true;
        }
        else
        {
            light2D.gameObject.SetActive(false);
            flashCollider.enabled = false;
        }
    }

    void RotateWeapon()
    {
        if (mainCamera is null) return;
        
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        //RotateWeapon();
    }
}
