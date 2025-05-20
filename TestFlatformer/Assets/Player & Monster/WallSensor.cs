using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WallSensor : MonoBehaviour
{
    [Tooltip("Ground ���̾�")]
    public LayerMask groundLayer;
    [HideInInspector] public bool touching = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & groundLayer) != 0)
            touching = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & groundLayer) != 0)
            touching = false;
    }
}

