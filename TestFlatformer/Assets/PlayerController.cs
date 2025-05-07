using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class CharacterController2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Jump Settings")]
    public float jumpForce = 10f;
    public float jumpAngle = 30f;
    public int maxJumpCount = 2;
    private int jumpCount = 0;
    private bool jumpRequest = false;

    [Header("Ground Settings")]
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private float facingDirection = 1f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        // 좌우 이동
        float h = Input.GetAxisRaw("Horizontal");
        if (h != 0) facingDirection = h;
        var lv = rb.linearVelocity;
        lv.x = h * moveSpeed;
        rb.linearVelocity = lv;

        // 점프 요청
        if (Input.GetButtonDown("Jump"))
            jumpRequest = true;
    }

    void FixedUpdate()
    {
        if (jumpRequest && jumpCount < maxJumpCount)
        {
            jumpCount++;
            float r = jumpAngle * Mathf.Deg2Rad;
            float vx = Mathf.Cos(r) * jumpForce * facingDirection;
            float vy = Mathf.Sin(r) * jumpForce;
            rb.linearVelocity = new Vector2(vx, vy);
        }
        jumpRequest = false;
    }

    // 땅에 닿았을 때 정확히 한 번만 호출
    void OnCollisionEnter2D(Collision2D col)
    {
        if ((groundLayer & (1 << col.gameObject.layer)) != 0)
        {
            jumpCount = 0;
        }
    }
}



