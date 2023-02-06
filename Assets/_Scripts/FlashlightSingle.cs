using System;
using System.Collections.Generic;
using System.Numerics;
using FMOD.Studio;
using FMODUnity;
using PlayerController;
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
    //flashlight sound
    [SerializeField] private ScriptableStats _stats;
    private EventInstance beamSound;
    private bool isPlaying = false;
    
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
    private List<Star> starsLit;

    private List<StarConnector> connectorsLit;
    
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

    [SerializeField] private LayerMask layerMask;
    
    // DEBUG

    public GameObject c1, c2;

    private void Start()
    {
        starsLit = new List<Star>();
        connectorsLit = new List<StarConnector>();
        beamSound = FMODUnity.RuntimeManager.CreateInstance(_stats.playerFlashlightSound);
        
        //controlScheme = player.ControlScheme == 1 ? "Keyboard" : "Gamepad";

        foreach (var i in InputSystem.devices)
        {
            //Debug.Log(i);
        }
    }

    private void OnEnable()
    {
        player = GetComponentInParent<PlayerController.PlayerController>();
        playerCamera = Camera.main;
        polygonCollider = GetComponent<PolygonCollider2D>();
        polygonCollider.enabled = false;
        /*
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        
        playerInputActions.Player.Flashlight.performed += Flashlight_performed;
        */
    }

    private void OnDisable()
    {
        //playerInputActions.Player.Disable();
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

    public void FlashlightAction()
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
        
        //FMODUnity.RuntimeManager.PlayOneShot(_stats.playerFlashlightSound);
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
        
        //Debug.Log(Camera.main.ScreenToWorldPoint(screenPos));
    }

    public void FlashLightOn()
    {
        beamSound.start();
        flashlightToggle = true;
        polygonCollider.enabled = true;
        player.CanMove = false;
        light2D.enabled = true;
        GameManager.Instance.UpdateGameState(GameManager.GameState.Flashlight);
    }

    public void FlashLightOff()
    {
        beamSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        flashlightToggle = false;
        polygonCollider.enabled = false;
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
        
        flashLeft = Quaternion.AngleAxis(rotation.z + light2D.pointLightOuterAngle / 2, Vector3.forward).normalized * transform.right * light2D.pointLightOuterRadius;
        flashRight = Quaternion.AngleAxis(rotation.z - light2D.pointLightOuterAngle / 2, Vector3.forward).normalized * transform.right * light2D.pointLightOuterRadius;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (currentStarCluster != null) return;
        if (other.CompareTag("StarCluster"))
        {
            currentGrabbableStarCluster = other.GetComponent<StarCluster>();
            clusterGrabbable = true;
        }

        if (other.CompareTag("Star"))
        {
            starsLit.Add(other.GetComponent<Star>());
        }

        if (other.CompareTag("StarConnector"))
        {
            connectorsLit.Add(other.GetComponent<StarConnector>());
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!flashlightToggle) return;

        if (!other.CompareTag("StarConnector")) return;
        if (currentStarCluster != null) return;

        StarConnector connector = other.GetComponent<StarConnector>();

        Vector2?[] points = new Vector2?[2];

        /*RaycastHit2D hit1 = Physics2D.Raycast(transform.position, flashLeft.normalized, light2D.pointLightOuterRadius, layerMask);
            if (hit1.collider != null)
            {
                Debug.Log(hit1.transform.GetInstanceID());
                points[0] = hit1.point;
            }
            else points[0] = null; 

            RaycastHit2D hit2 = Physics2D.Raycast(transform.position, flashRight.normalized, light2D.pointLightOuterRadius, layerMask);
            if (hit2.collider != null)
            {
                Debug.Log(hit2.transform.GetInstanceID());
                points[1] = hit2.point;
            }
            else points[1] = null;

            //Debug.Log(points[0] + " || " + points[1]);
            
            if (c1 != null) c1.transform.position = hit1.point;
            if (c2 != null) c2.transform.position = hit2.point;

            if (connectorsLit.Count > 1)
            {
                if (hit1.collider != null)
                {
                    if (hit1.transform.GetComponent<StarConnector>() != connector)
                    {
                        points[0] = null;
                    }
                }

                if (hit2.collider != null)
                {
                    if (hit2.transform.GetComponent<StarConnector>() != connector)
                    {
                        points[1] = null;
                    }
                }
            }
            else if (starsLit.Count > 1)
            {
                List<Star> starsLitSorted = starsLit;
                starsLitSorted.Sort((a, b) => a.count.CompareTo(b.count));
                
                // left
                if (hit1.collider == null && hit2.collider != null)
                {
                    if (connector.clusterPart == StarConnector.ClusterPart.Beginning)
                    {
                        points[0] = connector.star2.transform.position; //
                        points[1] = connector.star1.transform.position;
                    }
                    else if (connector.clusterPart == StarConnector.ClusterPart.End)
                    {
                        points[0] = connector.star1.transform.position; //
                        points[1] = connector.star2.transform.position;
                    }
                    else
                    {
                        points[1] = connector.star2.transform.position;
                    }
                    connector.EnableCollider();
                }
                
                // right
                if (hit1.collider != null && hit2.collider == null)
                {
                    if (connector.clusterPart == StarConnector.ClusterPart.Beginning)
                    {
                        points[0] = connector.star1.transform.position;
                        points[1] = connector.star2.transform.position; //
                    }
                    else if (connector.clusterPart == StarConnector.ClusterPart.End)
                    {
                        points[0] = connector.star2.transform.position;
                        points[1] = connector.star1.transform.position; //
                    }
                    else
                    {
                        points[0] = connector.star1.transform.position;
                    }
                    connector.EnableCollider();
                }
            }
            */

        bool hit1Found = false;
        bool hit2Found = false;
        RaycastHit2D hit1 = default, hit2 = default;

        RaycastHit2D[] hits1 = Physics2D.RaycastAll(transform.position, flashLeft.normalized, light2D.pointLightOuterRadius, layerMask);

        Debug.Log(hits1.Length);
        foreach (var hit in hits1)
        {
            if (hit.collider.GetComponent<StarConnector>() != connector) continue;
            
            points[0] = hit.point;
            hit1 = hit;
            hit1Found = true;
            break;
        }
        if (!hit1Found)
        {
            points[0] = null;
        }

        RaycastHit2D[] hits2 = Physics2D.RaycastAll(transform.position, flashRight.normalized, light2D.pointLightOuterRadius, layerMask);
        foreach (var hit in hits2)
        {
            if (hit.collider.GetComponent<StarConnector>() != connector) continue;

            points[1] = hit.point;
            hit2 = hit;
            hit2Found = true;
            break;
        }
        if (!hit2Found)
        {
            points[1] = null;
        }
        
        //Debug.Log(hit1Found + " " + hit2Found);
        
        if (connectorsLit.Count > 1)
        {
            if (hit1.collider != null)
            {
                if (hit1.transform.GetComponent<StarConnector>() != connector)
                {
                    points[0] = null;
                }
            }

            if (hit2.collider != null)
            {
                if (hit2.transform.GetComponent<StarConnector>() != connector)
                {
                    points[1] = null;
                }
            }
        }
        else if (starsLit.Count > 1)
        {
            List<Star> starsLitSorted = starsLit;
            starsLitSorted.Sort((a, b) => a.count.CompareTo(b.count));

            // left
            if (hit1.collider == null && hit2.collider != null)
            {
                if (connector.clusterPart == StarConnector.ClusterPart.Beginning)
                {
                    points[0] = connector.star2.transform.position; //
                    points[1] = connector.star1.transform.position;
                }
                else if (connector.clusterPart == StarConnector.ClusterPart.End)
                {
                    points[0] = connector.star1.transform.position; //
                    points[1] = connector.star2.transform.position;
                }
                else
                {
                    points[1] = connector.star2.transform.position;
                }

                connector.EnableCollider();
            }

            // right
            if (hit1.collider != null && hit2.collider == null)
            {
                if (connector.clusterPart == StarConnector.ClusterPart.Beginning)
                {
                    points[0] = connector.star1.transform.position;
                    points[1] = connector.star2.transform.position; //
                }
                else if (connector.clusterPart == StarConnector.ClusterPart.End)
                {
                    points[0] = connector.star2.transform.position;
                    points[1] = connector.star1.transform.position; //
                }
                else
                {
                    points[0] = connector.star1.transform.position;
                }

                connector.EnableCollider();
            }
        }
        
        //Debug.Log(points[0] + " || " + points[1]);
        
        other.GetComponent<StarConnector>().UpdateActiveColliderSpace(points, starsLit);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("StarCluster"))
        {
            currentGrabbableStarCluster = null;
            clusterGrabbable = false;
        }
        
        if (other.CompareTag("Star"))
        {
            starsLit.Remove(other.GetComponent<Star>());
        }

        if (other.CompareTag("StarConnector"))
        {
            connectorsLit.Remove(other.GetComponent<StarConnector>());
            other.GetComponent<StarConnector>().DisableCollider();
        }
    }

    void OnDrawGizmos()
    {
        //Gizmos.color = Color.green;
        //Gizmos.DrawRay(transform.position,flashLeft.normalized * light2D.pointLightOuterRadius);
        //Gizmos.DrawRay(transform.position,flashRight.normalized * light2D.pointLightOuterRadius);

    }
}