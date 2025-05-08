using UnityEngine;

/// <summary>
/// 중앙 입력 서비스 인터페이스
/// </summary>
public interface IInputService
{
    float Horizontal { get; }
    bool JumpPressed { get; }
    void Tick();
}

/// <summary>
/// 커스텀 InputManager (Singleton)
/// </summary>
public class InputManager : MonoBehaviour, IInputService
{
    public static InputManager Instance { get; private set; }
    public float Horizontal { get; private set; }
    public bool JumpPressed { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        Tick();
    }

    public void Tick()
    {
        // 좌우 입력
        Horizontal = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            Horizontal = -1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            Horizontal = 1f;
        // 점프 입력
        JumpPressed = Input.GetKeyDown(KeyCode.W);
    }
}
