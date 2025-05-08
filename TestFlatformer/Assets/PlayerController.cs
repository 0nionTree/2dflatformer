using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class CPlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Jump Settings")]
    public float jumpForce = 10f;
    public float jumpAngle = 30f;
    public int maxJumpCount = 2;

    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float tapThreshold = 0.3f;

    [Header("Ground Settings")]
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private float facingDirection = 1f;
    private int jumpCount = 0;
    private bool jumpRequest = false;

    // Dash state
    private bool isDashing = false;
    private float dashTimeLeft = 0f;
    private float originalGravityScale;

    // Double-tap trackers
    private float lastLeftTapTime = 0f;
    private int leftTapCount = 0;
    private float lastRightTapTime = 0f;
    private int rightTapCount = 0;

    // Fixed-angle jump override
    private bool isJumping = false;
    private int jumpOverrideFrames = 0;
    private int maxJumpOverrideFrames;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // ȸ�� ����
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        // ���� �߷� ����
        originalGravityScale = rb.gravityScale;
    }

    void Update()
    {
        float h = InputManager.Instance.Horizontal;

        // AŰ ���� �� ����
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (Time.time - lastLeftTapTime <= tapThreshold) leftTapCount++; else leftTapCount = 1;
            lastLeftTapTime = Time.time;
            if (leftTapCount == 2)
            {
                facingDirection = -1f;
                StartDash();
                leftTapCount = 0;
            }
        }
        // DŰ ���� �� ����
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (Time.time - lastRightTapTime <= tapThreshold) rightTapCount++; else rightTapCount = 1;
            lastRightTapTime = Time.time;
            if (rightTapCount == 2)
            {
                facingDirection = 1f;
                StartDash();
                rightTapCount = 0;
            }
        }

        // �뽬 �� ���� ���� �ƴ� �� �Ϲ� �̵�
        if (!isDashing && !isJumping)
        {
            if (h != 0f) facingDirection = Mathf.Sign(h);
            Vector2 linearVelocity = rb.linearVelocity;
            linearVelocity.x = h * moveSpeed;
            rb.linearVelocity = linearVelocity;
        }

        // ���� ��û
        if (InputManager.Instance.JumpPressed && jumpCount < maxJumpCount)
            jumpRequest = true;
    }

    void FixedUpdate()
    {
        // �뽬 ó��: �߷� ����
        if (isDashing)
        {
            dashTimeLeft -= Time.fixedDeltaTime;
            if (dashTimeLeft <= 0f)
            {
                isDashing = false;
                rb.gravityScale = originalGravityScale; // �߷� ����
            }
            else
            {
                // ���� �뽬 �� �߷� ���� ����
                rb.linearVelocity = new Vector2(facingDirection * dashSpeed, 0f);
                return;
            }
        }

        // Fixed-angle ���� �������̵�
        if (isJumping)
        {
            if (jumpOverrideFrames > 0)
            {
                rb.linearVelocity = new Vector2(
                    Mathf.Cos(jumpAngle * Mathf.Deg2Rad) * jumpForce * facingDirection,
                    Mathf.Sin(jumpAngle * Mathf.Deg2Rad) * jumpForce);
                jumpOverrideFrames--;
                return;
            }
            else isJumping = false;
        }

        // ���� ó��
        if (jumpRequest)
        {
            jumpCount++;
            isJumping = true;
            maxJumpOverrideFrames = Mathf.CeilToInt(
                (jumpForce * Mathf.Sin(jumpAngle * Mathf.Deg2Rad))
                / (Physics2D.gravity.y * Time.fixedDeltaTime * -1));
            jumpOverrideFrames = maxJumpOverrideFrames;
            jumpRequest = false;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if ((groundLayer.value & (1 << col.gameObject.layer)) != 0)
        {
            jumpCount = 0;
            isJumping = false;
        }
    }

    /// <summary>
    /// �뽬 ����: �߷� ���� ��� ����
    /// </summary>
    void StartDash()
    {
        isDashing = true;
        dashTimeLeft = dashDuration;
        rb.gravityScale = 0f;  // ���� �뽬 �� �߷� ���� ����
    }

    void OnDrawGizmosSelected()
    {
        // �ʿ� �� ������ Gizmos �߰�
    }
}




