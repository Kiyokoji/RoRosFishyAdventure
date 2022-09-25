using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class FlashlightSingle : MonoBehaviour
{
    private SinglePlayer player;

    private PlayerInputActions playerInputActions;

    Vector3 mousePos;

    public GameObject flashLight;

    private bool flashlightToggle = false;

    [HideInInspector] public bool isToggled = false;

    private void OnEnable()
    {
        player = GetComponentInParent<SinglePlayer>();

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Flashlight.performed += Flashlight_performed;
    }

    private void OnDisable()
    {
        playerInputActions.Player.Disable();
    }

    private void Flashlight_performed(InputAction.CallbackContext obj)
    {
        if(!player.isGrounded) return;

        flashlightToggle = !flashlightToggle;

        if (flashlightToggle)
        {
            flashLight.GetComponent<Light2D>().enabled = true;
            GameManager.instance.UpdateGameState(GameManager.GameState.Idle);
        } else
        {
            flashLight.GetComponent<Light2D>().enabled = false;
            GameManager.instance.UpdateGameState(GameManager.GameState.Moving);
        }
    }

    private void Update()
    {
        RotateWeapon();

        mousePos = playerInputActions.Player.MousePos.ReadValue<Vector2>();
    }

    void RotateWeapon()
    {
        if (Camera.main is { })
        {
            Vector2 direction = Camera.main.ScreenToWorldPoint(mousePos) - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = rotation;
        }
    }
}