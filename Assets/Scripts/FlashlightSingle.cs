using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class FlashlightSingle : NetworkBehaviour
{
    private SinglePlayer player;
    private Camera mainCam;
    private PlayerInputActions playerInputActions;
    private Vector3 mousePos;
    public GameObject flashLight;
    private bool flashlightToggle;

    [HideInInspector] public bool isToggled;
    private Light2D light2D;

    private void Start()
    {
        light2D = flashLight.GetComponent<Light2D>();
    }

    private void OnEnable()
    {
        player = GetComponentInParent<SinglePlayer>();
        mainCam = Camera.main;
        
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
        if (!IsOwner) return;
        if(!player.isGrounded) return;
        if (GameManager.Instance.state == GameManager.GameState.Paused) return;

        ToggleFlashlightServerRpc();
    }

    [ServerRpc]
    private void ToggleFlashlightServerRpc()
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
        if (!IsOwner) return;
        if (!flashlightToggle) return;
        
        RotateWeapon();
        
        mousePos = playerInputActions.Player.MousePos.ReadValue<Vector2>();
    }
    
    public void FlashLightOn()
    {
        flashlightToggle = true;
        player.canMove = false;
        light2D.enabled = true;
        GameManager.Instance.UpdateGameState(GameManager.GameState.Flashlight);
    }

    public void FlashLightOff()
    {
        flashlightToggle = false;
        //player.canMove = true;
        light2D.enabled = false;
        GameManager.Instance.UpdateGameState(GameManager.GameState.Moving);
    }

    private void RotateWeapon()
    {
        if (!mainCam) return;
        
        Vector2 direction = mainCam.ScreenToWorldPoint(mousePos) - transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;
    }
}