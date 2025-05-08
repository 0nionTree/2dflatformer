using UnityEngine;

/// <summary>
/// �߾� �Է� ���� �������̽�
/// </summary>
public interface IInputService
{
    float Horizontal { get; }
    bool JumpPressed { get; }
    void Tick();
}

/// <summary>
/// Ŀ���� InputManager (Singleton)
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
        // �¿� �Է�
        Horizontal = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            Horizontal = -1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            Horizontal = 1f;
        // ���� �Է�
        JumpPressed = Input.GetKeyDown(KeyCode.W);
    }
}
