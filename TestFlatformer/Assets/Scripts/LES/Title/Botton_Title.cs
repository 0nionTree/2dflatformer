using UnityEngine;
using UnityEngine.EventSystems;

public class Botton_Title : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int bottonNum;               // 버튼 번호 1 : 게임 시작, 2 : 이어하기, 3 : 새 게임
    public TitleManager TitleManager;   // 타이틀매니저 할당

    public bool canUseBotton = true;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (canUseBotton)
        {   // 버튼이 사용 가능한 상태일 경우
            // 타이틀 매니저에 bottonNum을 넘겨주고 버튼 기능은 타이틀 매니저에서 관리
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
