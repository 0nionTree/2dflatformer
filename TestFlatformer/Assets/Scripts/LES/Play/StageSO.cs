using UnityEngine;

[CreateAssetMenu(fileName = "Stage", menuName = "Scriptable Objects/Stage")]
public class StageSO : ScriptableObject
{
    public int level;
    public Vector2 start = new Vector2();

    public Vector2 minCameraBounds = new Vector2(0, -10);
    public Vector2 maxCameraBounds = new Vector2(0, 10);
}
