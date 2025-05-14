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
    public static PlayerData currentPlayerData;         // �÷������� ������ ������

    void Awake()
    {
        // �̱��� ����: ���� ������ �ϳ��� DataManager�� �����ϵ��� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ���� ����Ǿ DataManager�� ������
        }
        else
        {
            Destroy(gameObject); // ���� �ν��Ͻ��� ������ ���� ������ ���� ����
        }
    }

    // --- ���̺� ������ ���� �Լ� ---
    public void SaveGame()  // ���� ����
    {
        if(currentPlayerData == null)
        {
            Debug.Log("���̺� ���� �� ���� ����");
            currentPlayerData = CreateNewSaveData(); // �⺻ ���̺� ����
        }

        currentPlayerData.lastSaveTime = System.DateTime.Now.ToString();
        string json = JsonConvert.SerializeObject(currentPlayerData, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(savePath, json);
        Debug.Log("���̺� ���� �Ϸ�");
    }

    public void LoadSave()  // ���̺� �ε�
    {
        if (File.Exists(savePath)) // ���� ���� ���� Ȯ��
        {
            string json = File.ReadAllText(savePath); // ���� �б�
            currentPlayerData = JsonConvert.DeserializeObject<PlayerData>(json); // JSON �� ��ü ��ȯ
            Debug.Log("���̺� �ε� �Ϸ�");
        }
        else
        {
            Debug.Log("���̺� ���� �� ���� ����");
            currentPlayerData = CreateNewSaveData(); // �⺻ ���̺� ����

            string json = JsonConvert.SerializeObject(currentPlayerData, Newtonsoft.Json.Formatting.Indented); // ��ü �� JSON ���ڿ�
            File.WriteAllText(savePath, json); // JSON ����
            Debug.Log("���̺� ���� �Ϸ�");
        }
        Debug.Log("������" + savePath);
    }

    public bool isSave()
    {
        return File.Exists(savePath);
    }

    private static PlayerData CreateNewSaveData()   // �� ���̺� �ۼ�
    {
        PlayerData data = new PlayerData();

        data.lastSaveTime = System.DateTime.Now.ToString();
        data.StageLevel = 0;

        return data;
    }
}
