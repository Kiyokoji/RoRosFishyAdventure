using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class Flashlight : NetworkBehaviour
{
    private PlayerInputActions playerInputActions;

    private Vector3 mousePos;

    public GameObject flashLight;

    private bool flashlightToggle;

    //List<GameObject> currentCollision = new List<GameObject>();

    //public LayerMask hitmask;
    //private PolygonCollider2D flashCollider;

    //public Transform lightPoint;
    //public GameObject spriteMask;
    
    //public bool FlashlightOn = false;
    //private UnityEngine.Rendering.Universal.Light2D light2D;
    //private float intensityTemp;
    //private PolygonCollider2D platformCollider;

    //public float xOffset = 0.248f;

    [HideInInspector] public bool isToggled = false;

    private void OnEnable()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Player.Disable();
    }

    private void Update()
    {
        //FlashlightCheck();
    }

    private void Start()
    {
        //flashCollider = GetComponent<PolygonCollider2D>();
        //light2D = GetComponentInChildren<UnityEngine.Rendering.Universal.Light2D>();
        //platformCollider = GetComponent<PolygonCollider2D>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData networkInputData))
        {
            mousePos = networkInputData.mousePos;

            flashlightToggle = networkInputData.getFlashlightToggle;

            if (flashlightToggle)
            {
                EnableFlashlight();
            } else
            {
                DisableFlashlight();
            }
        }

        //Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.right) * 20f, Color.red);
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.right), 20f, hitmask);
        RotateWeapon();
        
    }

    private void EnableFlashlight()
    {
        flashLight.GetComponent<Light2D>().enabled = true;
    }

    private void DisableFlashlight()
    {
        flashLight.GetComponent<Light2D>().enabled = false;
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

    /*

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("LightPlatform1") && !isPlayer2)
        {
            currentCollision.Add(collision.gameObject);

            foreach (GameObject gObject in currentCollision)
            {
                gObject.GetPhotonView().RPC("EnablePlatform", RpcTarget.All);
            }
        } else if (collision.CompareTag("LightPlatform2") && isPlayer2)
        {
            currentCollision.Add(collision.gameObject);

            foreach (GameObject gObject in currentCollision)
            {
                gObject.GetPhotonView().RPC("EnablePlatform", RpcTarget.All);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("LightPlatform1") && !isPlayer2)
        {
            //currentCollision.Remove(collision.gameObject);
            //collision.gameObject.GetComponent<PlatformEnable>.DisablePlatform();
        } else if (collision.CompareTag("LightPlatform2") && isPlayer2)
        {
            //currentCollision.Remove(collision.gameObject);

            //collision.gameObject.GetPhotonView().RPC("DisablePlatform", RpcTarget.All);
        }
    }

    */

}
