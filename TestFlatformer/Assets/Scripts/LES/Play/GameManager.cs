using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject PlayerCharacter;

    public int stageLevel = 0;
    public List<StageSO> stageList = new List<StageSO>();

    public Vector2 playerVector;

    public CameraFollow CameraFollow;

    private void Awake()
    {
        stageLevel = DataManager.currentPlayerData.StageLevel;
    }

    private void Start()
    {
        CameraFollow.minBounds = stageList[stageLevel].minCameraBounds;
        Debug.Log(stageList[stageLevel].name);
        Debug.Log(stageList[stageLevel].minCameraBounds);
        CameraFollow.maxBounds = stageList[stageLevel].maxCameraBounds;
    }
}
