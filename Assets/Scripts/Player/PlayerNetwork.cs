using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Fusion;

public class PlayerNetwork : NetworkBehaviour
{
    PlayerNetwork playerNetwork;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 8f;
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
    [ReadOnly] public bool isGrounded;
    const float groundCheckRadius = 0.3f;

    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private bool isJumping;

    private InputAction move, jump;
    private PlayerInputActions playerInputActions;

    // Jump button input
    private bool jumpButtonDown = false;
    private bool jumpButtonUp = false;
    private bool jumpButtonHold = false;

    private bool flip;

    private Rigidbody2D rb;
    private Animator animator;
    public SpriteRenderer sprite;

    private void Awake()
    {
        playerNetwork =  GetComponent<PlayerNetwork>();
        animator      =  GetComponentInChildren<Animator>();
        rb            =  GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        move = playerInputActions.Player.Movement;
    }

    private void OnDisable()
    {
        playerInputActions.Player.Disable();
    }

    private void Start()
    {
    }

    private void LateUpdate()
    {
        /*
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
        */
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData networkInputData))
        {
            jumpButtonDown = networkInputData.getJumpDown;
            jumpButtonUp   = networkInputData.getJumpUp;
            jumpButtonHold = networkInputData.getJumpHold;

            moveDirection  = networkInputData.movementInput.x;
        }

        JumpCheck();
        GroundCheck();
        Move();
        JumpPhysics();
    }

    private void Move()
    {
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

        if (moveDirection != 0)
        {
            if (moveDirection > 0)
            {
                sprite.flipX = false;
            }

            if (moveDirection < 0)
            {
                sprite.flipX = true;
            }
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
            coyoteTimeCounter -= Runner.DeltaTime;
        }

        if (jumpButtonDown)
        {
            jumpTimeCounter = jumpTime;
            jumpBufferCounter = jumpButterTime;
        }
        else
        {
            jumpBufferCounter -= Runner.DeltaTime;
        }

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            isJumping = true;
            Jump();
            jumpBufferCounter = 0f;
        }

        if (jumpButtonHold && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                Jump();
                jumpTimeCounter -= Runner.DeltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (jumpButtonUp)
        {
            isJumping = false;
            coyoteTimeCounter = 0f;
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

}
