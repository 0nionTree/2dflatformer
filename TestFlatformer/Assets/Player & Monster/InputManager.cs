using UnityEngine;

public interface IInputService
{
    float Horizontal { get; }
    bool JumpPressed { get; }
    bool JumpHeld { get; }
    void Tick();
}

public class InputManager : MonoBehaviour, IInputService
{
    public static InputManager Instance { get; private set; }

    public float Horizontal { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool JumpHeld { get; private set; }

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
        Horizontal = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            Horizontal = -1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            Horizontal = 1f;

        JumpPressed = Input.GetKeyDown(KeyCode.W);
        JumpHeld = Input.GetKey(KeyCode.W);
    }
}