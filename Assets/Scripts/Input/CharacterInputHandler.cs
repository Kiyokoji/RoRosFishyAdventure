using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInputHandler : MonoBehaviour
{
    //float moveInputFloat;
    Vector2 moveInputVector;

    PlayerInputActions player;
    InputAction movement, jump, flashlight;

    bool jumpInputDown = false;
    bool jumpInputUp = false;
    bool jumpInputHold = false;

    bool flashlightToggle = true;

    Vector3 mousePosInput;

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        movement = playerInputActions.Player.Movement;
        jump = playerInputActions.Player.Jump;

        jump.performed += Jump;
        jump.started += JumpStarted;
        jump.canceled += JumpCanceled;

        playerInputActions.Player.Flashlight.performed += FlashlightPerformed;
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        jumpInputDown = true;
    }

    private void JumpStarted(InputAction.CallbackContext obj)
    {
        jumpInputHold = true;
        jumpInputUp = false;
    }

    private void JumpCanceled(InputAction.CallbackContext obj)
    {
        jumpInputUp = true;
        jumpInputDown = false;
        jumpInputHold = false;
    }


    private void FlashlightPerformed(InputAction.CallbackContext obj)
    {
        flashlightToggle = !flashlightToggle;
    }


    void Update()
    {
        //Move Input
        moveInputVector = movement.ReadValue<Vector2>();

        //Mouse Input
        mousePosInput = playerInputActions.Player.MousePos.ReadValue<Vector2>();



    }

    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();

        //Move Data
        networkInputData.movementInput = moveInputVector;

        //Jump Data
        networkInputData.getJumpDown = jumpInputDown;
        networkInputData.getJumpUp   = jumpInputUp;
        networkInputData.getJumpHold = jumpInputHold;

        //Mouse Data
        networkInputData.mousePos = mousePosInput;

        //Flashlight Data
        networkInputData.getFlashlightToggle = flashlightToggle;


        //Reset variables now that we have read their states to properly update input loop

        //flashlightToggle = false;
        jumpInputDown = false;
        moveInputVector = new Vector2(0, 0);


        return networkInputData;
    }
}
