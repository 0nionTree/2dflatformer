using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    // 카메라가 따라갈 대상 (플레이어)
    public Transform target;

    // 현재 카메라가 제한될 최소 좌표 (좌, 우측 하단)
    public Vector2 minBounds;
    public Vector2 maxBounds;

    // 카메라 제한을 사용할지 여부
    public bool useBounds = true;

    // 카메라 화면 절반 너비와 높이 (계산용)
    private float cameraHalfWidth;
    private float cameraHalfHeight;

    // 카메라 따라가기 속도 (값이 작을수록 더 부드럽게 따라감)
    public float followSmoothSpeed = 0.125f;

    // SmoothDamp에 필요한 속도 벡터 저장용 변수
    private Vector3 velocity = Vector3.zero;

    // 카메라 제한 전환 시 실행 중인 코루틴을 추적하기 위한 변수
    private Coroutine transitionCoroutine;

    // 시작 시 카메라의 크기를 기준으로 화면 절반 크기 계산
    void Start()
    {
        Camera cam = GetComponent<Camera>();

        // Orthographic 카메라에서 화면 절반 높이는 orthographicSize
        cameraHalfHeight = cam.orthographicSize;

        // 화면 절반 너비는 높이 * 비율(aspect)
        cameraHalfWidth = cameraHalfHeight * cam.aspect;
    }

    // 모든 업데이트가 끝난 후 카메라 위치 조정 (LateUpdate는 움직임 후 호출됨)
    void LateUpdate()
    {
        if (target == null) return;

        // 기본적으로 플레이어 위치를 따라가되 z축(깊이)은 카메라의 원래 값을 유지
        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

        // 제한이 활성화되어 있으면 min/maxBounds를 기준으로 Clamp
        if (useBounds)
        {
            // x축과 y축 각각에서 카메라 절반 크기를 고려하여 범위 제한
            float clampedX = Mathf.Clamp(desiredPosition.x, minBounds.x + cameraHalfWidth, maxBounds.x - cameraHalfWidth);
            float clampedY = Mathf.Clamp(desiredPosition.y, minBounds.y + cameraHalfHeight, maxBounds.y - cameraHalfHeight);

            // 제한된 좌표로 목표 위치 수정
            desiredPosition = new Vector3(clampedX, clampedY, desiredPosition.z);
        }

        // SmoothDamp로 현재 위치에서 목표 위치까지 부드럽게 이동
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, followSmoothSpeed);
    }

    // 외부에서 카메라 제한 범위를 즉시 설정하는 함수
    public void SetBounds(Vector2 min, Vector2 max)
    {
        minBounds = min;
        maxBounds = max;
    }

    // 외부에서 카메라 제한을 켜거나 끌 수 있는 함수
    public void EnableBounds(bool enable)
    {
        useBounds = enable;
    }

    // 새로운 제한 범위를 지정하고, 해당 영역 중앙으로 부드럽게 이동하는 함수 (duration 초 동안)
    public void MoveToTargetBounds(Vector2 newMin, Vector2 newMax, float duration)
    {
        // 이전에 실행 중인 전환 코루틴이 있다면 멈춤
        if (transitionCoroutine != null)
            StopCoroutine(transitionCoroutine);

        // 새 코루틴 시작
        transitionCoroutine = StartCoroutine(SmoothTransitionToBounds(newMin, newMax, duration));
    }

    // 카메라가 기존 제한 중앙 → 새 제한 중앙으로 부드럽게 이동하는 코루틴
    private IEnumerator SmoothTransitionToBounds(Vector2 newMin, Vector2 newMax, float duration)
    {
        // 현재 제한의 중앙 위치 계산
        Vector2 currentCenter = (minBounds + maxBounds) / 2f;

        // 새로 지정된 제한의 중앙 위치 계산
        Vector2 targetCenter = (newMin + newMax) / 2f;

        float timer = 0f;

        // duration 동안 보간 진행
        while (timer < duration)
        {
            timer += Time.deltaTime;

            // 부드러운 보간 (S자 곡선 형태의 이동)
            float t = Mathf.SmoothStep(0f, 1f, timer / duration);

            // 현재와 목표 중심 사이를 보간하여 새로운 위치 계산
            Vector2 midPoint = Vector2.Lerp(currentCenter, targetCenter, t);

            // z축은 그대로 유지한 채 카메라 이동
            transform.position = new Vector3(midPoint.x, midPoint.y, transform.position.z);

            yield return null;
        }

        // 마지막에 정확히 목표 위치로 이동 보정 + 새 제한 적용
        transform.position = new Vector3(targetCenter.x, targetCenter.y, transform.position.z);
        SetBounds(newMin, newMax);
    }
}
