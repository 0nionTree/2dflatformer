using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour
{
    private Animator animator;
    private Character character;
    private SpriteRenderer spriteRenderer;

    // Animation parameter names
    private static readonly string SPEED = "Speed";
    private static readonly string IS_GROUNDED = "IsGrounded";
    private static readonly string IS_WALL_SLIDING = "IsWallSliding";
    private static readonly string IS_DASHING = "IsDashing";
    private static readonly string IS_ATTACKING = "IsAttacking";
    private static readonly string IS_WAITING = "IsWaiting";
    private static readonly string HURT = "Hurt";
    private static readonly string DIE = "Die";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        character = GetComponent<Character>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (character == null) return;

        // 기본 애니메이션 파라미터 업데이트
        animator.SetFloat(SPEED, Mathf.Abs(character.GetComponent<Rigidbody2D>().linearVelocity.x));
        animator.SetBool(IS_GROUNDED, character.IsGrounded);
        animator.SetBool(IS_WALL_SLIDING, character.IsTouchingWall && !character.IsGrounded);

        // 플레이어 전용 애니메이션
        if (character is Player player)
        {
            animator.SetBool(IS_DASHING, player.IsDashing);
        }

        // 적 전용 애니메이션
        if (character is Enemy enemy)
        {
            animator.SetBool(IS_ATTACKING, enemy.IsAttacking);
            animator.SetBool(IS_WAITING, enemy.IsWaiting);
        }
    }

    public void PlayHurtAnimation()
    {
        animator.SetTrigger(HURT);
    }

    public void PlayDeathAnimation()
    {
        animator.SetTrigger(DIE);
    }

    public void FlipSprite(bool faceRight)
    {
        spriteRenderer.flipX = !faceRight;
    }
} 