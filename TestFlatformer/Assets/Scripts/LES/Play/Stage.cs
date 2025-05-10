using UnityEngine;

[CreateAssetMenu(fileName = "Stage", menuName = "Scriptable Objects/Stage")]
public class Stage : ScriptableObject
{
    public int level;
    public float startX;
    public float startY;

    public Vector2 minCameraBounds = new Vector2(0, -10);
    public Vector2 maxCameraBounds = new Vector2(0, 10);
}
