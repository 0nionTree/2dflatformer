using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [Header("Enemy Settings")]
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float chaseSpeed = 4f;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float waitTimeAtPoint = 2f;

    // Enemy state
    private enum State { Patrol, Chase, Attack, Wait }
    private State currentState;
    private Transform player;
    private int currentPatrolIndex;
    private float waitTimer;
    private float attackTimer;
    private bool isWaiting;

    // Properties
    public bool IsAttacking => currentState == State.Attack;
    public bool IsWaiting => isWaiting;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        currentState = State.Patrol;
        currentPatrolIndex = 0;
    }

    private void Update()
    {
        if (isDead) return;

        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                CheckForPlayer();
                break;
            case State.Chase:
                ChasePlayer();
                CheckAttackRange();
                break;
            case State.Attack:
                Attack();
                break;
            case State.Wait:
                Wait();
                break;
        }

        UpdateAnimations();
    }

    private void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        Vector2 direction = (targetPoint.position - transform.position).normalized;

        // 방향 전환
        if (direction.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (direction.x < 0 && isFacingRight)
        {
            Flip();
        }

        // 이동
        rb.linearVelocity = new Vector2(direction.x * patrolSpeed, rb.linearVelocity.y);

        // 목적지 도달 체크
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            currentState = State.Wait;
            waitTimer = waitTimeAtPoint;
            isWaiting = true;
        }
    }

    private void ChasePlayer()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;

        // 방향 전환
        if (direction.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (direction.x < 0 && isFacingRight)
        {
            Flip();
        }

        // 추적
        rb.linearVelocity = new Vector2(direction.x * chaseSpeed, rb.linearVelocity.y);
    }

    private void Attack()
    {
        rb.linearVelocity = Vector2.zero;
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0)
        {
            // 공격 실행
            if (player != null)
            {
                float distance = Vector2.Distance(transform.position, player.position);
                if (distance <= attackRange)
                {
                    player.GetComponent<Player>()?.TakeDamage(10f);
                }
            }
            attackTimer = attackCooldown;
            currentState = State.Chase;
        }
    }

    private void Wait()
    {
        rb.linearVelocity = Vector2.zero;
        waitTimer -= Time.deltaTime;

        if (waitTimer <= 0)
        {
            isWaiting = false;
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            currentState = State.Patrol;
        }
    }

    private void CheckForPlayer()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= detectionRange)
        {
            currentState = State.Chase;
        }
    }

    private void CheckAttackRange()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= attackRange)
        {
            currentState = State.Attack;
            attackTimer = attackCooldown;
        }
        else if (distance > detectionRange)
        {
            currentState = State.Patrol;
        }
    }

    private void UpdateAnimations()
    {
        if (animator == null) return;

        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        animator.SetBool("IsAttacking", IsAttacking);
        animator.SetBool("IsWaiting", IsWaiting);
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        // 감지 범위 표시
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // 공격 범위 표시
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // 순찰 경로 표시
        if (patrolPoints != null)
        {
            Gizmos.color = Color.blue;
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                if (patrolPoints[i] != null)
                {
                    Gizmos.DrawSphere(patrolPoints[i].position, 0.2f);
                    if (i < patrolPoints.Length - 1 && patrolPoints[i + 1] != null)
                    {
                        Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i + 1].position);
                    }
                }
            }
        }
    }
} 