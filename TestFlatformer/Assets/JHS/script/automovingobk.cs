using TMPro;
using UnityEngine;

public class automovingobk : MonoBehaviour
{
    public Vector3 moveOffset = new Vector3(3f, 0f, 0f);
    public float moveSpeed = 2f;
    private Vector3 pointA;
    private Vector3 pointB;
    private Vector3 target;
    [SerializeField] private GameObject player;

    void Start()
    {
        pointA = transform.position;
        pointB = pointA + moveOffset;
        target = pointB;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            target = (target == pointA) ? pointB : pointA;
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            player.transform.parent = transform;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            player.transform.parent = null;
        }
    }
}