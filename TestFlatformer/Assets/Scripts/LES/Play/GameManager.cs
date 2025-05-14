using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject PlayerCharacter;

    public int stageLevel = 0;
    public List<StageSO> stageList = new List<StageSO>();

    public Transform characterTransform;

    public CameraFollow CameraFollow;

    private void Awake()
    {
        stageLevel = DataManager.currentPlayerData.StageLevel;
    }

    private void Start()
    {
        CameraBoundSetting();
        characterTransform.position = stageList[stageLevel].start;
    }

    private void Update()
    {
        Debug.Log(characterTransform.position.x);
        Debug.Log(stageList[stageLevel].maxCameraBounds.x);
        if (characterTransform.position.x < stageList[stageLevel].minCameraBounds.x ||
            characterTransform.position.x > stageList[stageLevel].maxCameraBounds.x ||
            characterTransform.position.y < stageList[stageLevel].minCameraBounds.y ||
            characterTransform.position.y > stageList[stageLevel].maxCameraBounds.y)
        {   // 카메라 범위에서 캐릭터가 벗어나면
            Debug.Log("스테이지 진행");
            int x = stageLevel + 1;
            Debug.Log(stageList[x].maxCameraBounds.x);
            if (characterTransform.position.x > stageList[x].minCameraBounds.x &&
                characterTransform.position.x < stageList[x].maxCameraBounds.x &&
                characterTransform.position.y > stageList[x].minCameraBounds.y &&
                characterTransform.position.y < stageList[x].maxCameraBounds.y)
            {
                Debug.Log("진행 성공");
                stageLevel = x;
                CameraBoundSetting();
            }
        }
    }

    private void CameraBoundSetting()
    {
        CameraFollow.minBounds = stageList[stageLevel].minCameraBounds;
        CameraFollow.maxBounds = stageList[stageLevel].maxCameraBounds;
    }
}
