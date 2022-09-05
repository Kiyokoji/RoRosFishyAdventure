using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine.Experimental.Rendering.Universal;

public class FlashLight : MonoBehaviour
{
    List<GameObject> currentCollision = new List<GameObject>();

    public LayerMask hitmask;
    private PolygonCollider2D flashCollider;

    public Transform lightPoint;
    public GameObject spriteMask;
    
    public bool FlashlightOn = false;
    private Light2D light2D;
    private float intensityTemp;
    private PolygonCollider2D platformCollider;

    public float xOffset = 0.248f;

    [HideInInspector] public bool isToggled = false;
    private bool isPlayer2;
    public PlayerController controller;

    PhotonView view;

    private void Start()
    {
        flashCollider = GetComponent<PolygonCollider2D>();
        isPlayer2 = controller.Player2;
        view = GetComponent<PhotonView>();
        light2D = GetComponentInChildren<Light2D>();
        platformCollider = GetComponent<PolygonCollider2D>();

        if (FlashlightOn) return;
        intensityTemp = light2D.intensity;
        light2D.intensity = 0;
        spriteMask.SetActive(false);
        platformCollider.enabled = false;
    }

    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.right) * 20f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.right), 20f, hitmask);

        if (view.IsMine)
        {
            RotateWeapon();

            if (hit)
            {
                if (hit.transform.CompareTag("Ground"))
                {   
                    view.RPC("DisableFlashlight", RpcTarget.All);
                }
                else if (isPlayer2 && hit.transform.CompareTag("SwitchPlatform1"))
                {
                    view.RPC("DisableFlashlight", RpcTarget.All);
                }
                else if (!isPlayer2 && hit.transform.CompareTag("SwitchPlatform2"))
                {
                    view.RPC("DisableFlashlight", RpcTarget.All);
                }
                else
                {
                    if (FlashlightOn)
                    {
                        view.RPC("EnableFlashlight", RpcTarget.All);
                    }
                }
            }
        }
    }

    [PunRPC]
    void DisableFlashlight()
    {
        spriteMask.SetActive(false);
        flashCollider.enabled = false;
    }
    
    [PunRPC]
    void EnableFlashlight()
    {
        spriteMask.SetActive(true);
        flashCollider.enabled = true;
    }

    void RotateWeapon()
    {
        if (Camera.main is { })
        {
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = rotation;
        }
    }

    [PunRPC]
    [Button(ButtonSizes.Large), GUIColor(0, 1, 1)]
    public void ToggleFlashlight()
    {
        if (FlashlightOn)
        {
            FindObjectOfType<AudioManager>().Play("ClickOff");
            intensityTemp = light2D.intensity;
            light2D.intensity = 0;
            FlashlightOn = false;
            spriteMask.SetActive(false);
            platformCollider.enabled = true;
            platformCollider.enabled = false;
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("ClickOn");
            light2D.intensity = intensityTemp;
            FlashlightOn = true;
            spriteMask.SetActive(true);
            platformCollider.enabled = false;
            platformCollider.enabled = true;
        }
    }

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
            currentCollision.Remove(collision.gameObject);

            collision.gameObject.GetPhotonView().RPC("DisablePlatform", RpcTarget.All);
        } else if (collision.CompareTag("LightPlatform2") && isPlayer2)
        {
            currentCollision.Remove(collision.gameObject);

            collision.gameObject.GetPhotonView().RPC("DisablePlatform", RpcTarget.All);
        }
    }

}
