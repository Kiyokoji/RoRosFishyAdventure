using System;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class FlashlightSingle : MonoBehaviour
{
    private PlayerController.PlayerController player;
    public Camera playerCamera;
    private PlayerInputActions playerInputActions;
    private Vector3 mousePos;
    public GameObject flashlight;
    private bool flashlightToggle;
    
    private bool clusterGrabbable;
    private StarCluster currentStarCluster, currentGrabbableStarCluster;
    private PolygonCollider2D polygonCollider;
    
    private Light2D light2D;

    private Vector2 flashLeft, flashRight;

    private void Start()
    {
        light2D = flashlight.GetComponent<Light2D>();
        polygonCollider = GetComponent<PolygonCollider2D>();
    }

    private void OnEnable()
    {
        player = GetComponentInParent<PlayerController.PlayerController>();
        playerCamera = Camera.main;
        
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
        if(!player.Grounded) return;
        if (GameManager.Instance.state == GameManager.GameState.Paused) return;

        if (clusterGrabbable) GrabCluster();
        else if (currentStarCluster != null) ClusterAction();
        else ToggleFlashlight();
    }
    
    private void ToggleFlashlight()
    {
        flashlightToggle = !flashlightToggle;
        
        if (flashlightToggle)
        {
            FlashLightOn();
        } else
        {
            FlashLightOff();
        }
    }

    private void Update()
    {
        if (!flashlightToggle) return;
        
        RotateWeapon();
        //UpdateCluster();
        
        mousePos = playerInputActions.Player.MousePos.ReadValue<Vector2>();

        //Vector3 dir = Quaternion.AngleAxis(transform.eulerAngles.z + light2D.pointLightOuterAngle / 2)
        flashLeft = Quaternion.AngleAxis(transform.rotation.z + light2D.pointLightOuterAngle / 2, Vector3.forward) * transform.right * light2D.pointLightOuterRadius;
        flashRight = Quaternion.AngleAxis(transform.rotation.z - light2D.pointLightOuterAngle / 2, Vector3.forward) * transform.right * light2D.pointLightOuterRadius;
    }
    
    public void FlashLightOn()
    {
        flashlightToggle = true;
        player.CanMove = false;
        light2D.enabled = true;
        GameManager.Instance.UpdateGameState(GameManager.GameState.Flashlight);
    }

    public void FlashLightOff()
    {
        flashlightToggle = false;
        player.CanMove = true;
        light2D.enabled = false;
        GameManager.Instance.UpdateGameState(GameManager.GameState.Moving);
    }

    private void RotateWeapon()
    {
        if (!playerCamera) return;
        
        Vector2 direction = playerCamera.ScreenToWorldPoint(mousePos + playerCamera.WorldToScreenPoint(playerCamera.transform.localPosition)) - transform.localPosition;
        //Debug.Log(mainCam.ScreenToWorldPoint(player.transform.position) + " || " + mainCam.ScreenToWorldPoint(mousePos) + " || " + direction);
        //Debug.Log(playerCamera.ScreenToWorldPoint(mousePos));
        
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;
    }
    
    // -----------------
    // STAR CLUSTERS
    // -----------------

    private void UpdateCluster()
    {
        clusterGrabbable = currentStarCluster == null;
    }

    private void GrabCluster()
    {
        if (!currentGrabbableStarCluster) return;

        clusterGrabbable = false;
        currentStarCluster = currentGrabbableStarCluster;
        currentStarCluster.unplacedContainer.transform.SetParent(transform);
    }

    private void ClusterAction()
    {
        if (currentStarCluster.ClusterSize > 0)
        {
            currentStarCluster.NextStar();
        }
        else
        {
            currentStarCluster.unplacedContainer.transform.SetParent(currentStarCluster.transform);
            currentStarCluster = null;
        }

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (currentStarCluster != null) return;
        if (col.CompareTag("StarCluster"))
        {
            currentGrabbableStarCluster = col.GetComponent<StarCluster>();
            clusterGrabbable = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("StarConnector"))
        {
            Vector2[] points = new Vector2[2];
            
            RaycastHit2D hit1 = Physics2D.Raycast(transform.position, flashLeft);
            if (hit1.collider != null) points[0] = hit1.point;
            Debug.Log(hit1.point);
            
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position, flashRight);
            if (hit2.collider != null) points[1] = hit2.point;
            
            other.GetComponent<StarConnector>().UpdateActiveColliderSpace(points);
        }

        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("StarCluster"))
        {
            currentGrabbableStarCluster = null;
            clusterGrabbable = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        //Gizmos.DrawRay(transform.position,flashLeft * light2D.pointLightOuterRadius);
    }
}