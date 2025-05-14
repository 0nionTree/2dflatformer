using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public List<GameObject> title_Botton = new List<GameObject>();
    private Color enterColor = new Color(0.75f, 0.75f, 0.75f);
    private Color exitColor = new Color(1f, 1f, 1f);
    private Color lockColor = new Color(0.4f, 0.4f, 0.4f);

    public void BottonClickEvent(GameObject Botton, int BTNum)
    {
        switch (BTNum)
        {
            case 0:
                title_Botton[0].SetActive(false);
                title_Botton[1].SetActive(true);
                title_Botton[2].SetActive(true);
                if (!(DataManager.Instance.isSave()))   // ���� : ���̺� ������ �������
                {
                    title_Botton[2].GetComponent<Botton_Title>().canUseBotton = false;
                    title_Botton[2].GetComponent<Image>().color = lockColor;
                }
                break;
            case 1:
                DataManager.Instance.SaveGame();
                SceneLoader.Instance.LoadScene("PlayScene");
                break;
            case 2:
                DataManager.Instance.LoadSave();
                SceneLoader.Instance.LoadScene("PlayScene");
                break;
        }
    }

    public void BottonEnterEvent(GameObject Botton, int BTNum)
    {
        Image BottonImage = Botton.GetComponent<Image>();
        BottonImage.color = enterColor; // ���콺 �ø��� ��ư �� ���� ȸ������ ����

        switch (BTNum)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
        }
    }

    public void BottonExitEvent(GameObject Botton, int BTNum)
    {
        Image BottonImage = Botton.GetComponent<Image>();
        BottonImage.color = exitColor; // ���콺 ġ��� ��ư �� ���� ȸ������ ����

        switch (BTNum)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
        }
    }
}