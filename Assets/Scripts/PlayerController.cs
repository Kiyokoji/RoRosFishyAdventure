using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Cinemachine;
using ExitGames.Client.Photon.StructWrapping;
using Level;
using ParadoxNotion.Design;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerController : MonoBehaviour
{
    private PlayerInputActions pInputActions;
    private InputAction movement, jump, flashlightInput, interact;

    public ParticleSystem dust;

    [UnityEngine.Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public bool facingRight = true;
    private float moveDirection;
    [ReadOnly] public bool isGrounded;

    //Coyote time
    [SerializeField] private float coyoteTime = 0.1f;
    private float coyoteTimeCounter;

    //Jump buffer
    [SerializeField] private float jumpButterTime = 0.1f;
    private float jumpBufferCounter;

    private bool standingOnCrate;
    public bool canMove = true;
    public bool canUseFlashlight = true;

    [UnityEngine.Header("Physics")]
    //public float maxSpeed = 7f;
    public LayerMask groundLayer;
    public float linearDrag = 4f;
    public float gravity = 1;
    public float fallMultiplier = 5;
    private Rigidbody2D rb;
    private CapsuleCollider2D mainCollider;
    [SerializeField] Transform groundCheckCollider;
    const float groundCheckRadius = 0.3f;

    // Jump button input
    private bool jumpButtonHeld = false;
    private bool jumpButtonReleased = false;

    private bool isJumping = false;

    [UnityEngine.Header("Crates")]
    [ReadOnly] public bool hasCrate;
    [ReadOnly] public Transform currentCrate;
    public float spitForce = 5f;
    public Transform spitOffset;

    [UnityEngine.Header("Cinemachine")]
    public CinemachineVirtualCamera cam;
    public Transform skeleton;
    PhotonView view;
    public bool Player2 = false;

    private List<Collider2D> interactibleList;
    private List<SpriteRenderer> spriteRenderers;
    public FlashLight flashLight;
    private Animator animator;

    [HideInInspector] public Transform crateTransform;
    [HideInInspector] public bool isMe;

    List<string> currentCollision = new List<string>();

    private void Awake()
    {
        pInputActions = new PlayerInputActions();
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<CapsuleCollider2D>();
        view = GetComponent<PhotonView>();
        flashLight = GetComponentInChildren<FlashLight>();

        spriteRenderers = new List<SpriteRenderer>();
        interactibleList = new List<Collider2D>();

        foreach (Transform t in GetComponentInChildren<Animator>().transform)
        {
            if (!t.GetComponent<SpriteRenderer>()) continue;
            spriteRenderers.Add(t.GetComponent<SpriteRenderer>());
        }

        facingRight = transform.localScale.x > 0;

        canMove = !flashLight.FlashlightOn;
        canUseFlashlight = true;

        // Checks if PhotonView is "mine"
        if (!view.IsMine)
        {
            cam.enabled = false;
            isMe = false;
        }
        else
        {
            isMe = true;
        }

        animator.SetBool("Ground", true);
        animator.SetBool("Walking", false);
    }

    private void OnEnable()
    {
        movement = pInputActions.Player.Movement;
        movement.Enable();

        jump = pInputActions.Player.Jump;
        jump.performed += JumpInput;
        jump.started += JumpButtonHoldStart;
        jump.canceled += JumpButtonHoldEnd;
        jump.Enable();

        flashlightInput = pInputActions.Player.FlashlightToggle;
        flashlightInput.performed += ToggleFlashlight;
        flashlightInput.Enable();

        interact = pInputActions.Player.Interact;
        interact.performed += Interact;
        interact.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
        jump.Disable();
        flashlightInput.Disable();
        interact.Disable();
    }

    private void FixedUpdate()
    {
        if (view.IsMine)
        {
            Move(moveDirection);
            ModifyPhysics();
            MoveDirection();
        }
    }

    private void Update()
    {
        if (!view.IsMine) return;
        if (!canMove) return;
        GroundCheck();
        JumpCheck();

        if (hasCrate)
        {
            currentCrate.transform.position = spitOffset.transform.position;
        }
    }

    private void LateUpdate()
    {
        if (isGrounded)
        {
            animator.SetBool("Ground", true);
        }
        else
        {
            animator.SetBool("Ground", false);
        }

        if (rb.velocity.magnitude > 0.1)
        {
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }
    }

    private void MoveDirection()
    {
        moveDirection = movement.ReadValue<Vector2>().x;

        // Changes sprite orientation depending on the facing direction
        if (flashLight.FlashlightOn)
        {
            if (Mathf.Abs(flashLight.transform.rotation.z) < 0.7f)
            {
                //facingRight = true;
                //spriteRenderer.transform.localRotation = Quaternion.Euler(0, 0, 0);
                //view.RPC("Flip", RpcTarget.All);
            }

            if (Mathf.Abs(flashLight.transform.rotation.z) > 0.7f)
            {
                //facingRight = false;
                //spriteRenderer.transform.localRotation = Quaternion.Euler(0, 180, 0);
                //view.RPC("Flip", RpcTarget.All);
            }
            //Debug.Log(flashLight.transform.rotation.z);
        }

        if (moveDirection != 0 && !flashLight.FlashlightOn)
        {
            if (moveDirection > 0 && !facingRight) // || Mathf.Abs(flashLight.transform.rotation.z) <= 0.5f
            {
                //facingRight = true;
                view.RPC("ChangeFacingDirection", RpcTarget.All, true);
                //spriteRenderer.transform.localRotation = Quaternion.Euler(0, 0, 0);
                view.RPC("Flip", RpcTarget.All);
            }

            if (moveDirection < 0 && facingRight) // || Mathf.Abs(flashLight.transform.rotation.z) > 0.5f
            {
                //facingRight = false;
                view.RPC("ChangeFacingDirection", RpcTarget.All, false);
                //spriteRenderer.transform.localRotation = Quaternion.Euler(0, 180, 0);
                view.RPC("Flip", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void ChangeFacingDirection(bool right)
    {
        facingRight = right;
    }

    private void GroundCheck()
    {
        isGrounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius - Physics.defaultContactOffset, groundLayer);

        if (colliders.Length > 0)
        {
            isGrounded = true;
            jumpButtonReleased = false;
        }

        if (!isGrounded)
        {
            standingOnCrate = false;
        }
    }

    private void JumpInput(InputAction.CallbackContext ctx)
    {
        // (!canMove || !isGrounded) return;
        //Jumping = true;
    }

    private void JumpCheck()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpButterTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f && !isJumping)
        {
            Jump();
            jumpBufferCounter = 0f;
            StartCoroutine(JumpCooldown());
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            coyoteTimeCounter = 0f;
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        //rb.velocity = Vector2.up * jumpForce;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        animator.SetTrigger("Jump");
        view.RPC("CreateDust", RpcTarget.All);
    }

    private IEnumerator JumpCooldown()
    {
        isJumping = true;
        yield return new WaitForSeconds(0.4f);
        isJumping = false;
    }

    private void Interact(InputAction.CallbackContext ctx)
    {
        if (!view.IsMine) return;

        if (hasCrate && currentCrate != null)
        {
            FindObjectOfType<AudioManager>().Play("Spit");
            view.RPC("SpitCrate", RpcTarget.All);
        }
        else
        {
            Collider2D[] colliders = interactibleList.ToArray();

            if (colliders.Length <= 0) return;
            foreach (var t in colliders)
            {
                if (t == mainCollider) continue;
                if (t.CompareTag("Crate") && !standingOnCrate)
                {
                    FindObjectOfType<AudioManager>().Play("Nom");
                    view.RPC("SwallowCrate", RpcTarget.All, t.GetComponent<PhotonView>().ViewID);
                }
                break;
            }
        }
    }

    [PunRPC]
    private void SwallowCrate(int viewID)
    {
        currentCrate = PhotonView.Find(viewID).transform;
        hasCrate = true;

        if(view.IsMine)
        currentCrate.GetComponent<TransferOwnership>().RequestOwnership();

        currentCrate.GetComponent<PhotonView>().RPC(!Player2 ? "ParentPlayer1" : "ParentPlayer2", RpcTarget.All);
        currentCrate.GetComponent<PhotonView>().RPC("DisableCrate", RpcTarget.All);

        //skeleton.transform.RotateAround(transform.position, transform.up, transform.position.x > c.position.x ? 180f : 0f);
        if (transform.position.x > currentCrate.position.x && facingRight)
        {
            view.RPC("Flip", RpcTarget.All);
            //facingRight = false;
            view.RPC("ChangeFacingDirection", RpcTarget.All, false);
        }

        if (transform.position.x < currentCrate.position.x && !facingRight)
        {
            view.RPC("Flip", RpcTarget.All);
            //facingRight = true;
            view.RPC("ChangeFacingDirection", RpcTarget.All, true);
        }
        animator.Play("Nom");
    }

    [PunRPC]
    private void SpitCrate()
    {
        // Debug.Log(currentCrate.transform.position);
        currentCrate.GetComponent<PhotonView>().RPC("UnparentPlayer", RpcTarget.All);
        currentCrate.GetComponent<PhotonView>().RPC("EnableCrate", RpcTarget.All);
        currentCrate.transform.rotation = Quaternion.Euler(0, 0, 0);

        //Vector3 offset = spitOffset.localPosition;
        //currentCrate.position = transform.position + new Vector3(facingRight ? offset.x : -offset.x, offset.y, 0);
        currentCrate.GetComponent<Rigidbody2D>().AddForce(facingRight ? Vector2.right * spitForce : Vector2.left * spitForce, ForceMode2D.Impulse);
        //Debug.Log(currentCrate.GetComponent<Rigidbody2D>().velocity);
        //Debug.Log(currentCrate != null);
        hasCrate = false;
        currentCrate = null;

        

        animator.Play("Nom");
    }

    private void Move(float horizontal)
    {
        //if (isGrounded) 
        //{
        //rb.AddForce(Vector2.right * (horizontal * moveSpeed), ForceMode2D.Force);
        //if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        //{
        //rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        //}
        //}

        // TODO some sort of deceleration maybe
        if (!canMove) return;
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
    }

    private void ModifyPhysics()
    {
        bool changingDirections = moveDirection > 0 && rb.velocity.x < 0 || moveDirection < 0 && rb.velocity.x > 0;

        if (isGrounded)
        {
            if (Mathf.Abs(moveDirection) < 0.4f || changingDirections)
            {
                rb.drag = linearDrag;
            }
            else
            {
                rb.drag = 0;
            }

            rb.gravityScale = 2;
        }
        else
        {
            rb.gravityScale = gravity;
            rb.drag = linearDrag * 0.15f;

            if (rb.velocity.y < 0 || jumpButtonReleased)
            {
                rb.gravityScale = fallMultiplier;
                //rb.gravityScale = gravity * fallMultiplier;
            }
            else if (rb.velocity.y > 0 && !jumpButtonHeld)
            {
                rb.gravityScale = gravity;
                //rb.gravityScale = gravity * (fallMultiplier / 2);
            }
        }
    }

    private void JumpButtonHoldStart(InputAction.CallbackContext ctx)
    {
        jumpButtonHeld = true;
    }

    private void JumpButtonHoldEnd(InputAction.CallbackContext ctx)
    {
        jumpButtonHeld = false;
        jumpButtonReleased = true;
    }

    private void ToggleFlashlight(InputAction.CallbackContext ctx)
    {
        if (!isGrounded || !canUseFlashlight) return;

        canMove = flashLight.FlashlightOn;

        //Debug.Log(!lightShine.FlashlightOn + " " + canMove);

        if (!canMove) rb.velocity = Vector2.zero;
        flashLight.GetComponent<PhotonView>().RPC("ToggleFlashlight", RpcTarget.All);
    }

    [PunRPC]
    private void Flip()
    {
        //var crateObj = transform.Find("Crate");
        //var crateObj2 = transform.Find("Crate2");

        skeleton.transform.RotateAround(transform.position, transform.up, 180f);
        spitOffset.transform.RotateAround(transform.position, transform.up, 180f);
        /*

        if (crateObj != null)
        crateObj.transform.RotateAround(transform.position, transform.up, 180f);
        if(crateObj2 != null)
        crateObj2.transform.RotateAround(transform.position, transform.up, 180f);

        */
        if (isGrounded)
        {
            view.RPC("CreateDust", RpcTarget.All);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Crate"))
        {
            interactibleList.Add(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Crate"))
        {
            interactibleList.Remove(other);
        }
    }

    [PunRPC]
    void CreateDust()
    {
        dust.Play();
    }
}
