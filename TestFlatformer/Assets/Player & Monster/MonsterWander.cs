using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class MonsterWander : MonoBehaviour
{
    public float horizontalSpeed = 3f;
    public float verticalSpeed = 2f;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public string tilemapLayerName = "Ground";
    public string playerLayerName = "Player";

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private int tilemapLayer;
    private int playerLayer;
    private int state;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        tilemapLayer = LayerMask.NameToLayer(tilemapLayerName);
        playerLayer = LayerMask.NameToLayer(playerLayerName);
        if (playerLayer >= 0)
            Physics2D.IgnoreLayerCollision(gameObject.layer, playerLayer, true);
            
    }

    void FixedUpdate()
    {
        Vector2 pos = rb.position;
        Vector2 dir;
        float speed = 0f;

        switch (state)
        {
            case 0:
                dir = Vector2.right;
                speed = horizontalSpeed;
                if (pos.x >= maxX) state = 1;
                break;
            case 1:
                dir = Vector2.up;
                speed = verticalSpeed;
                if (pos.y >= maxY) state = 2;
                break;
            case 2:
                dir = Vector2.left;
                speed = horizontalSpeed;
                if (pos.x <= minX) state = 3;
                break;
            case 3:
                dir = Vector2.down;
                speed = verticalSpeed;
                if (pos.y <= minY) state = 0;
                break;
            default:
                dir = Vector2.right;
                break;
        }
        spriteRenderer.flipX = dir.x > 0;

            Vector2 nextPos = pos + dir * speed * Time.fixedDeltaTime;
        rb.MovePosition(nextPos);
    }
}




