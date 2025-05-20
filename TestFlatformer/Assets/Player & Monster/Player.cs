using UnityEngine;
using System.Collections;

public class Player : Character
{
    [Header("Player Specific")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private float wallClimbSpeed = 3f;
    [SerializeField] private float wallSlideSpeed = 1.5f;
    [SerializeField] private float doubleJumpForce = 8f;
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private float wallJumpForce = 10f;  // 벽 점프 힘
    [SerializeField] private float wallJumpDirection = 0.5f;  // 벽 점프 시 반대 방향으로 튀어나가는 정도

    // Player state
    private bool isDashing;
    private float dashTimeLeft;
    private float dashCooldownLeft;
    private int jumpsLeft;
    private bool isWallSliding;
    private float wallDirectionX;
    private bool canWallJump = true;  // 벽 점프 가능 여부

    // Properties
    public bool IsDashing => isDashing;
    public bool IsWallSliding => isWallSliding;

    protected override void Awake()
    {
        base.Awake();
        jumpsLeft = maxJumps;
    }

    private void Update()
    {
        if (isDead) return;

        HandleInput();
        UpdateAnimations();
    }

    private void HandleInput()
    {
        // 이동 입력
        float moveInput = Input.GetAxisRaw("Horizontal");
        
        // 대시 입력
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownLeft <= 0)
        {
            StartDash();
        }

        // 점프 입력
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                Jump();
            }
            else if (isTouchingWall && canWallJump)  // 벽 점프 조건
            {
                WallJump();
            }
            else if (jumpsLeft > 0)
            {
                DoubleJump();
            }
        }

        // 벽 오르기
        if (isTouchingWall && !isGrounded)
        {
            HandleWallClimb();
        }
        else
        {
            isWallSliding = false;
        }

        // 이동 처리
        if (!isDashing)
        {
            Move(moveInput);
        }

        // 대시 쿨다운 업데이트
        if (dashCooldownLeft > 0)
        {
            dashCooldownLeft -= Time.deltaTime;
        }
    }

    private void Move(float moveInput)
    {
        // 방향 전환
        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }

        // 이동 속도 적용
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        jumpsLeft = maxJumps - 1;
        canWallJump = true;  // 지면에서 점프하면 벽 점프 가능
    }

    private void WallJump()
    {
        // 벽의 반대 방향으로 점프
        float jumpDirection = isFacingRight ? -1 : 1;
        rb.linearVelocity = new Vector2(jumpDirection * wallJumpDirection * moveSpeed, wallJumpForce);
        Flip();  // 벽 점프 시 방향 전환
        canWallJump = false;  // 벽 점프 후 일정 시간 동안 벽 점프 불가
        StartCoroutine(ResetWallJump());
    }

    private System.Collections.IEnumerator ResetWallJump()
    {
        yield return new WaitForSeconds(0.2f);  // 0.2초 후 벽 점프 가능
        canWallJump = true;
    }

    private void DoubleJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpForce);
        jumpsLeft--;
    }

    private void StartDash()
    {
        isDashing = true;
        dashTimeLeft = dashDuration;
        dashCooldownLeft = dashCooldown;
        rb.gravityScale = 0f;
    }

    private void HandleWallClimb()
    {
        float verticalInput = Input.GetAxisRaw("Vertical");
        
        if (verticalInput > 0)
        {
            // 벽 오르기
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, wallClimbSpeed);
            isWallSliding = false;
        }
        else if (verticalInput < 0 || rb.linearVelocity.y < 0)
        {
            // 벽 미끄러지기
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeed);
            isWallSliding = true;
        }
    }

    private void UpdateAnimations()
    {
        if (animator == null) return;

        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsWallSliding", isWallSliding);
        animator.SetBool("IsDashing", isDashing);
    }

    protected override void CheckGround()
    {
        base.CheckGround();
        if (isGrounded)
        {
            jumpsLeft = maxJumps;
            canWallJump = true;  // 지면에 닿으면 벽 점프 가능
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            dashTimeLeft -= Time.fixedDeltaTime;
            if (dashTimeLeft <= 0)
            {
                isDashing = false;
                rb.gravityScale = 1f;
            }
        }
    }

    public override void TakeDamage(float damage)
    {
        if (isDashing) return; // 대시 중에는 무적
        base.TakeDamage(damage);
    }
} 