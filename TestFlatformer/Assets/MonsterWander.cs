using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MonsterWander : MonoBehaviour
{
    public float speed = 3f;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    Rigidbody2D rb;
    int state = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void FixedUpdate()
    {
        Vector2 pos = rb.position;
        Vector2 dir;
        switch (state)
        {
            case 0: dir = Vector2.right; if (pos.x >= maxX) state = 1; break;
            case 1: dir = Vector2.up; if (pos.y >= maxY) state = 2; break;
            case 2: dir = Vector2.left; if (pos.x <= minX) state = 3; break;
            case 3: dir = Vector2.down; if (pos.y <= minY) state = 0; break;
            default: dir = Vector2.right; break;
        }
        rb.MovePosition(pos + dir * speed * Time.fixedDeltaTime);
    }
}



