using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class FlashlightSingle : MonoBehaviour
{
    private SinglePlayer player;
    private Camera mainCam;
    private PlayerInputActions playerInputActions;
    private Vector3 mousePos;
    public GameObject flashLight;
    private bool flashlightToggle;

    [HideInInspector] public bool isToggled;

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
        if(!player.isGrounded) return;
        if (GameManager.Instance.state == GameManager.GameState.Paused) return;

        flashlightToggle = !flashlightToggle;

        if (flashlightToggle)
        {
            flashLight.GetComponent<Light2D>().enabled = true;
            GameManager.Instance.UpdateGameState(GameManager.GameState.Idle);
        } else
        {
            flashLight.GetComponent<Light2D>().enabled = false;
            GameManager.Instance.UpdateGameState(GameManager.GameState.Moving);
        }
    }

    private void Update()
    {
        if (!flashlightToggle) return;
        
        RotateWeapon();
        
        mousePos = playerInputActions.Player.MousePos.ReadValue<Vector2>();
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