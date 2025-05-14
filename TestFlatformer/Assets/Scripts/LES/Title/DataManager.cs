using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class PlayerData
{
    public string lastSaveTime;
    public int StageLevel;
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public static string savePath => Application.persistentDataPath + "/save.json";
    public static PlayerData currentPlayerData;         // 플레이중인 유저의 데이터

    void Awake()
    {
        // 싱글턴 패턴: 게임 내에서 하나의 DataManager만 존재하도록 보장
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 변경되어도 DataManager가 유지됨
        }
        else
        {
            Destroy(gameObject); // 기존 인스턴스가 있으면 새로 생성된 것은 삭제
        }
    }

    // --- 세이브 데이터 관리 함수 ---
    public void SaveGame()  // 게임 저장
    {
        if(currentPlayerData == null)
        {
            Debug.Log("세이브 없음 → 새로 생성");
            currentPlayerData = CreateNewSaveData(); // 기본 세이브 생성
        }

        currentPlayerData.lastSaveTime = System.DateTime.Now.ToString();
        string json = JsonConvert.SerializeObject(currentPlayerData, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(savePath, json);
        Debug.Log("세이브 생성 완료");
    }

    public void LoadSave()  // 세이브 로드
    {
        if (File.Exists(savePath)) // 파일 존재 여부 확인
        {
            string json = File.ReadAllText(savePath); // 파일 읽기
            currentPlayerData = JsonConvert.DeserializeObject<PlayerData>(json); // JSON → 객체 변환
            Debug.Log("세이브 로드 완료");
        }
        else
        {
            Debug.Log("세이브 없음 → 새로 생성");
            currentPlayerData = CreateNewSaveData(); // 기본 세이브 생성

            string json = JsonConvert.SerializeObject(currentPlayerData, Newtonsoft.Json.Formatting.Indented); // 객체 → JSON 문자열
            File.WriteAllText(savePath, json); // JSON 저장
            Debug.Log("세이브 생성 완료");
        }
        Debug.Log("저장경로" + savePath);
    }

    public bool isSave()
    {
        return File.Exists(savePath);
    }

    private static PlayerData CreateNewSaveData()   // 새 세이브 작성
    {
        PlayerData data = new PlayerData();

        data.lastSaveTime = System.DateTime.Now.ToString();
        data.StageLevel = 0;

        return data;
    }
}
