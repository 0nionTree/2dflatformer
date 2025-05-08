using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerController: MonoBehaviour
{
    public float moveSpeed = 5f;
    public float stationaryJumpForce = 10f;
    public float movingJumpForce = 10f;
    public float jumpAngle = 30f;
    public int maxJumpCount = 2;
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float jumpHoldGravityScale = 0.5f;
    public float maxJumpHoldTime = 3f;
    public LayerMask groundLayer;

    Rigidbody2D rb;
    float facingDirection = 1f;
    float inputX;
    int jumpCount;
    bool jumpRequest;
    bool isDashing;
    float dashTimeLeft;
    float originalGravityScale;
    bool isJumping;
    float jumpHoldTimer;
    int jumpOverrideFrames;
    int maxJumpOverrideFrames;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        originalGravityScale = rb.gravityScale;
    }

    void Update()
    {
            inputX = InputManager.Instance.Horizontal;
            if (InputManager.Instance.JumpPressed)
                jumpRequest = true;
            if (InputManager.Instance.JumpHeld && isJumping)
                jumpHoldTimer += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.LeftShift))
                StartDash();
    }

    void FixedUpdate()
    {
        Debug.Log($"jumpReq={jumpRequest}, isJumping={isJumping}, jumpCount={jumpCount}");

        bool grounded = IsGrounded();
        if (grounded)
        {
            jumpCount = 0;
            isJumping = false;
        }

        if (jumpRequest && jumpCount < maxJumpCount)
        {
            jumpCount++;
            isJumping = true;
            jumpHoldTimer = 0f;
            // 가만히/이동 중 구분 없이 고정 각도 점프
            float angleRad = jumpAngle * Mathf.Deg2Rad;
            float vx = Mathf.Cos(angleRad) * movingJumpForce * Mathf.Sign(inputX == 0 ? facingDirection : inputX);
            float vy = Mathf.Sin(angleRad) * movingJumpForce;
            rb.linearVelocity = new Vector2(vx, vy);
            jumpRequest = false;
        }

        // 점프 홀드 중이면 중력 축소
        if (isJumping && InputManager.Instance.JumpHeld && jumpHoldTimer < maxJumpHoldTime)
            rb.gravityScale = jumpHoldGravityScale;
        else
            rb.gravityScale = originalGravityScale;

        // 기본 좌우 이동
        rb.linearVelocity = new Vector2(inputX * moveSpeed, rb.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if ((groundLayer.value & (1 << col.gameObject.layer)) != 0)
        {
            jumpCount = 0;
            isJumping = false;
        }
    }

    void StartDash()
    {
        isDashing = true;
        dashTimeLeft = dashDuration;
        rb.gravityScale = 0f;
    }

    bool IsGrounded() => Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer);
}
