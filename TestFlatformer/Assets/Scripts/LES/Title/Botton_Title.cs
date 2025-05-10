using UnityEngine;
using UnityEngine.EventSystems;

public class Botton_Title : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int bottonNum;               // ��ư ��ȣ 1 : ���� ����, 2 : �̾��ϱ�, 3 : �� ����
    public TitleManager TitleManager;   // Ÿ��Ʋ�Ŵ��� �Ҵ�

    public bool canUseBotton = true;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (canUseBotton)
        {   // ��ư�� ��� ������ ������ ���
            // Ÿ��Ʋ �Ŵ����� bottonNum�� �Ѱ��ְ� ��ư ����� Ÿ��Ʋ �Ŵ������� ����
            TitleManager.BottonClickEvent(this.gameObject, bottonNum);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {   
        if (canUseBotton)
        {   // 
            TitleManager.BottonEnterEvent(this.gameObject, bottonNum);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (canUseBotton)
        {   // 
            TitleManager.BottonExitEvent(this.gameObject, bottonNum);
        }
    }
}
