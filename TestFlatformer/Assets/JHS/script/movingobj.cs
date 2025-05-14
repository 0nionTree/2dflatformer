using UnityEngine;

public class movingobj : MonoBehaviour
{
    public Vector3 moveOffset = new Vector3(0f, 3f, 0f);
    public float moveSpeed = 2f;
    private Vector3 pointC;
    private Vector3 pointD;
    private Vector3 target;
    private bool playerOnPlatform = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pointC = transform.position;
        pointD = pointC + moveOffset;
        target = pointD;
    }

    void Update()
    {
        if (playerOnPlatform)
        {
            transform.position = Vector3.MoveTowards(transform.position, pointD, moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, pointC, moveSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player on platform!");
            playerOnPlatform = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player left platform!");
            playerOnPlatform = false;
        }
    }
}