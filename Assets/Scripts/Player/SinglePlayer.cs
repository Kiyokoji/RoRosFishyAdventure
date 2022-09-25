using FMOD;
using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SinglePlayer : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float moveAcceleration;
    [SerializeField] private float maxMoveSpeed;
    private bool facingRight = false;
    private bool canMove = true;
    private float moveDirection;

    [Header("Jump Physics")]
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpButterTime = 0.1f;
    [SerializeField] private float fallMultiplier = 6f;
    [SerializeField] private float linearDrag = 4f;
    [SerializeField] private float gravity = 3;
    [SerializeField] private float jumpTime = 0.2f;
    private float jumpTimeCounter;

    [SerializeField] Transform groundCheckCollider;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.3f;
    [ReadOnly] public bool isGrounded;


    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private bool isJumping;

    private InputAction move, jump;
    private PlayerInputActions playerInputActions;

    // Jump button input
    private bool jumpButtonDown = false;
    private bool jumpButtonUp = false;
    private bool jumpButtonHold = false;

    [Header("References")]
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private Transform skeleton;

    private void OnEnable()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        move = playerInputActions.Player.Movement;

        jump = playerInputActions.Player.Jump;

        jump.performed += JumpPerformed;
        jump.started += JumpStarted;
        jump.canceled += JumpCanceled;

        GameManager.gameStateChanged += GameManagerStateChanged;
    }

    private void OnDisable()
    {
        playerInputActions.Player.Disable();

        GameManager.gameStateChanged -= GameManagerStateChanged;
    }

    private void Awake()
    {
        animator = skeleton.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void GameManagerStateChanged(GameManager.GameState state)
    {
        if(state == GameManager.GameState.Moving)
        {
            canMove = true;
        }

        if(state == GameManager.GameState.Idle)
        {
            canMove = false;
            rb.velocity = Vector2.zero;
        }
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (!canMove) return;

        moveDirection = move.ReadValue<Vector2>().x;
        JumpCheck();
        GroundCheck();
    }

    private void FixedUpdate()
    {
        if (!canMove) return;

        Move();
        JumpPhysics();
    }

    private void LateUpdate()
    {

    }

    private void Move()
    {
        //Floaty movement
        /*    
        rb.AddForce(new Vector2(moveDirection, 0f) * moveAcceleration);

        if(Mathf.Abs(rb.velocity.x) > maxMoveSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxMoveSpeed, rb.velocity.y);
        }
        */

        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

        if (moveDirection > 0 && facingRight)
        {
            Flip();
        }

        if (moveDirection < 0 && !facingRight)
        {
            Flip();
        }
    }

    private void GroundCheck()
    {
        isGrounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius - Physics.defaultContactOffset, groundLayer);

        if (colliders.Length > 0)
        {
            isGrounded = true;
        }
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

        if (jumpButtonDown && isGrounded)
        {
            jumpTimeCounter = jumpTime;
            jumpBufferCounter = jumpButterTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            isJumping = true;
            //Jump();
            jumpBufferCounter = 0f;
        }

        if (jumpButtonHold && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                Jump();
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                jumpTimeCounter = 0;
                isJumping = false;
            }
        }

        if (jumpButtonUp)
        {
            isJumping = false;
            coyoteTimeCounter = 0f;
            jumpBufferCounter = 0f;
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        jumpButtonDown = false;
    }

    private void JumpPhysics()
    {
        bool changingDirections = moveDirection > 0 && rb.velocity.x < 0 || moveDirection < 0 && rb.velocity.x > 0;

        if (rb.velocity.y < 0.2)
        {
            jumpButtonHold = false;
            isJumping = false;
        }

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

            rb.gravityScale = gravity;
        }
        else
        {
            rb.gravityScale = gravity;
            rb.drag = linearDrag * 0.15f;

            if (rb.velocity.y < 0 || jumpButtonUp)
            {
                rb.gravityScale = fallMultiplier;
            }
            else if (rb.velocity.y > 0 && !jumpButtonHold)
            {
                rb.gravityScale = gravity;
            }
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        skeleton.transform.RotateAround(transform.position, transform.up, 180f);
    }

    private void JumpPerformed(InputAction.CallbackContext obj)
    {
        jumpButtonDown = true;
    }

    private void JumpStarted(InputAction.CallbackContext obj)
    {
        jumpButtonHold = true; jumpButtonUp = false;
    }

    private void JumpCanceled(InputAction.CallbackContext obj)
    {
        jumpButtonUp = true; jumpButtonHold = false; jumpButtonDown = false;
    }
}
