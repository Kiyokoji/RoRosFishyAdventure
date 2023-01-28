using System;
using System.Numerics;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.Rendering.Universal;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class FlashlightSingle : MonoBehaviour
{
    [Title("Game Objects")]
    [InfoBox("The Player Camera of the Player operating this flashlight", InfoMessageType.None)]
    [InfoBox("Put the Player Camera of the operating Player in here", InfoMessageType.Error, "IsPlayerCameraPrefabUsed")]
    [SerializeField]
    private Camera playerCamera;
    private bool IsPlayerCameraPrefabUsed() { return !(playerCamera != null); }
    
    [InfoBox("The Light2D object - basically the flashlight", InfoMessageType.None)]
    [InfoBox("Put the Light2D (found in the children) in here", InfoMessageType.Error, "IsLight2DPrefabUsed")]
    [SerializeField]
    private Light2D light2D;
    private bool IsLight2DPrefabUsed() { return !(light2D != null); }
    
    private PlayerController.PlayerController player;
    private PlayerInputActions playerInputActions;
    private Vector3 mousePos, screenPos, stickPos, lastStickPos;

    private StarCluster currentStarCluster, currentGrabbableStarCluster;
    private PolygonCollider2D polygonCollider;
    
    private bool flashlightToggle;
    public bool FlashlightToggle => flashlightToggle;
    
    private bool clusterGrabbable;

    private Vector2 flashLeft, flashRight;

    private string controlScheme;
    public string ControlScheme
    {
        get => controlScheme;
        set => controlScheme = value;
    }

    private void Start()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();

        //controlScheme = player.ControlScheme == 1 ? "Keyboard" : "Gamepad";

        foreach (var i in InputSystem.devices)
        {
            Debug.Log(i);
        }
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

        return;
        if (clusterGrabbable) GrabCluster();
        else if (currentStarCluster != null) ClusterAction();
        else ToggleFlashlight();
    }
    
    public void ToggleFlashlight()
    {
        if(!player.Grounded) return;
        if (GameManager.Instance.state == GameManager.GameState.Paused) return;
        
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
        UpdateScreenPos();
        
        RotateWeapon();
        
        //Debug.Log(playerCamera.WorldToScreenPoint(player.transform.position) + " | " + mousePos);

        //UpdateCluster();

        //Debug.Log(mousePos + " || " + playerCamera.transform.localPosition);
        //Vector3 dir = Quaternion.AngleAxis(transform.eulerAngles.z + light2D.pointLightOuterAngle / 2)
        //flashLeft = Quaternion.AngleAxis(transform.rotation.z + light2D.pointLightOuterAngle / 2, Vector3.forward) * transform.right * light2D.pointLightOuterRadius;
        //flashRight = Quaternion.AngleAxis(transform.rotation.z - light2D.pointLightOuterAngle / 2, Vector3.forward) * transform.right * light2D.pointLightOuterRadius;
    }

    public void UpdateInputPositions(Vector2 mouse, Vector2 stick)
    {
        mousePos = mouse;
        stickPos = stick != Vector2.zero? stick : lastStickPos;
        lastStickPos = stickPos;
    }

    private void UpdateScreenPos()
    {
        if (controlScheme == "Gamepad") return;
        //mousePos = playerInputActions.Player.MousePos.ReadValue<Vector2>();
        screenPos = mousePos;
        screenPos.z = playerCamera.nearClipPlane + Math.Abs(playerCamera.transform.position.z);
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

        Vector2 direction = controlScheme == "Keyboard" ? playerCamera.ScreenToWorldPoint(screenPos) - transform.position : stickPos;

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
        if (currentStarCluster.CurrentOperatedStar < currentStarCluster.ClusterSize)
        {
            currentStarCluster.NextStar();
        }

        if (currentStarCluster.CurrentOperatedStar == currentStarCluster.ClusterSize)
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
            if (currentStarCluster != null) return;
            
            Vector2?[] points = new Vector2?[2];
            
            RaycastHit2D hit1 = Physics2D.Raycast(transform.position, flashLeft);
            if (hit1.collider != null) points[0] = hit1.point;
            else points[0] = null;
            
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position, flashRight);
            if (hit2.collider != null) points[1] = hit2.point;
            else points[1] = null;
            
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