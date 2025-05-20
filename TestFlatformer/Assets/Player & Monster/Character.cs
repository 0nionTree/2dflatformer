using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Basic Stats")]
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float jumpForce = 10f;
    [SerializeField] protected float fallMultiplier = 2.5f;
    [SerializeField] protected float lowJumpMultiplier = 2f;

    [Header("Ground Check")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckRadius = 0.2f;
    [SerializeField] protected LayerMask groundLayer;

    [Header("Wall Check")]
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance = 0.5f;
    [SerializeField] protected LayerMask wallLayer;  // 벽 전용 레이어

    // Components
    protected Rigidbody2D rb;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected BoxCollider2D boxCollider;

    // State
    protected float currentHealth;
    protected bool isGrounded;
    protected bool isTouchingWall;
    protected bool isFacingRight = true;
    protected bool isDead;

    // Properties
    public bool IsGrounded => isGrounded;
    public bool IsTouchingWall => isTouchingWall;
    public bool IsFacingRight => isFacingRight;
    public bool IsDead => isDead;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        CheckGround();
        CheckWall();
        HandleFallMultiplier();
    }

    protected virtual void CheckGround()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }
    }

    protected virtual void CheckWall()
    {
        if (wallCheck != null)
        {
            // 벽 레이어로만 체크
            isTouchingWall = Physics2D.Raycast(wallCheck.position, 
                isFacingRight ? Vector2.right : Vector2.left, 
                wallCheckDistance, 
                wallLayer);
        }
    }

    protected virtual void HandleFallMultiplier()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    public virtual void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // 피격 효과
            StartCoroutine(FlashRed());
        }
    }

    protected virtual void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
        boxCollider.enabled = false;
        
        // 죽음 애니메이션 재생
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
    }

    protected System.Collections.IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    protected virtual void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        if (wallCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(wallCheck.position, 
                wallCheck.position + (isFacingRight ? Vector3.right : Vector3.left) * wallCheckDistance);
        }
    }
} 