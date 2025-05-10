using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    // ī�޶� ���� ��� (�÷��̾�)
    public Transform target;

    // ���� ī�޶� ���ѵ� �ּ� ��ǥ (��, ���� �ϴ�)
    public Vector2 minBounds;
    public Vector2 maxBounds;

    // ī�޶� ������ ������� ����
    public bool useBounds = true;

    // ī�޶� ȭ�� ���� �ʺ�� ���� (����)
    private float cameraHalfWidth;
    private float cameraHalfHeight;

    // ī�޶� ���󰡱� �ӵ� (���� �������� �� �ε巴�� ����)
    public float followSmoothSpeed = 0.125f;

    // SmoothDamp�� �ʿ��� �ӵ� ���� ����� ����
    private Vector3 velocity = Vector3.zero;

    // ī�޶� ���� ��ȯ �� ���� ���� �ڷ�ƾ�� �����ϱ� ���� ����
    private Coroutine transitionCoroutine;

    // ���� �� ī�޶��� ũ�⸦ �������� ȭ�� ���� ũ�� ���
    void Start()
    {
        Camera cam = GetComponent<Camera>();

        // Orthographic ī�޶󿡼� ȭ�� ���� ���̴� orthographicSize
        cameraHalfHeight = cam.orthographicSize;

        // ȭ�� ���� �ʺ�� ���� * ����(aspect)
        cameraHalfWidth = cameraHalfHeight * cam.aspect;
    }

    // ��� ������Ʈ�� ���� �� ī�޶� ��ġ ���� (LateUpdate�� ������ �� ȣ���)
    void LateUpdate()
    {
        if (target == null) return;

        // �⺻������ �÷��̾� ��ġ�� ���󰡵� z��(����)�� ī�޶��� ���� ���� ����
        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

        // ������ Ȱ��ȭ�Ǿ� ������ min/maxBounds�� �������� Clamp
        if (useBounds)
        {
            // x��� y�� �������� ī�޶� ���� ũ�⸦ ����Ͽ� ���� ����
            float clampedX = Mathf.Clamp(desiredPosition.x, minBounds.x + cameraHalfWidth, maxBounds.x - cameraHalfWidth);
            float clampedY = Mathf.Clamp(desiredPosition.y, minBounds.y + cameraHalfHeight, maxBounds.y - cameraHalfHeight);

            // ���ѵ� ��ǥ�� ��ǥ ��ġ ����
            desiredPosition = new Vector3(clampedX, clampedY, desiredPosition.z);
        }

        // SmoothDamp�� ���� ��ġ���� ��ǥ ��ġ���� �ε巴�� �̵�
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, followSmoothSpeed);
    }

    // �ܺο��� ī�޶� ���� ������ ��� �����ϴ� �Լ�
    public void SetBounds(Vector2 min, Vector2 max)
    {
        minBounds = min;
        maxBounds = max;
    }

    // �ܺο��� ī�޶� ������ �Ѱų� �� �� �ִ� �Լ�
    public void EnableBounds(bool enable)
    {
        useBounds = enable;
    }

    // ���ο� ���� ������ �����ϰ�, �ش� ���� �߾����� �ε巴�� �̵��ϴ� �Լ� (duration �� ����)
    public void MoveToTargetBounds(Vector2 newMin, Vector2 newMax, float duration)
    {
        // ������ ���� ���� ��ȯ �ڷ�ƾ�� �ִٸ� ����
        if (transitionCoroutine != null)
            StopCoroutine(transitionCoroutine);

        // �� �ڷ�ƾ ����
        transitionCoroutine = StartCoroutine(SmoothTransitionToBounds(newMin, newMax, duration));
    }

    // ī�޶� ���� ���� �߾� �� �� ���� �߾����� �ε巴�� �̵��ϴ� �ڷ�ƾ
    private IEnumerator SmoothTransitionToBounds(Vector2 newMin, Vector2 newMax, float duration)
    {
        // ���� ������ �߾� ��ġ ���
        Vector2 currentCenter = (minBounds + maxBounds) / 2f;

        // ���� ������ ������ �߾� ��ġ ���
        Vector2 targetCenter = (newMin + newMax) / 2f;

        float timer = 0f;

        // duration ���� ���� ����
        while (timer < duration)
        {
            timer += Time.deltaTime;

            // �ε巯�� ���� (S�� � ������ �̵�)
            float t = Mathf.SmoothStep(0f, 1f, timer / duration);

            // ����� ��ǥ �߽� ���̸� �����Ͽ� ���ο� ��ġ ���
            Vector2 midPoint = Vector2.Lerp(currentCenter, targetCenter, t);

            // z���� �״�� ������ ä ī�޶� �̵�
            transform.position = new Vector3(midPoint.x, midPoint.y, transform.position.z);

            yield return null;
        }

        // �������� ��Ȯ�� ��ǥ ��ġ�� �̵� ���� + �� ���� ����
        transform.position = new Vector3(targetCenter.x, targetCenter.y, transform.position.z);
        SetBounds(newMin, newMax);
    }
}
