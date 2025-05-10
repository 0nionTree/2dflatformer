using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector2 minBounds = new Vector2(0, 0);
    public Vector2 maxBounds = new Vector2(50, 30);

    private float cameraHalfWidth;
    private float cameraHalfHeight;

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        cameraHalfHeight = cam.orthographicSize;
        cameraHalfWidth = cameraHalfHeight * cam.aspect;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

        float clampedX = Mathf.Clamp(desiredPosition.x, minBounds.x + cameraHalfWidth, maxBounds.x - cameraHalfWidth);
        float clampedY = Mathf.Clamp(desiredPosition.y, minBounds.y + cameraHalfHeight, maxBounds.y - cameraHalfHeight);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}
