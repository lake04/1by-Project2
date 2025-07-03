using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform canvas;
    private Transform previousParent;
    private RectTransform rect;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>().transform;
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// 현재 오브젝트를 드래그하기 시작할 떄 1회 호출
    /// </summary>

    public void OnBeginDrag(PointerEventData eventData)
    {
        //드래그 직전에 소속되어 있던 부모 Trasnform 정보 저장
        previousParent = transform.parent;

        //현재 드래그중인 UI가 화면의 최상단에 출력되도록 하기 위해
        transform.SetParent(canvas);    //부모 오브젝트를 Canvas로 설정
        transform.SetAsLastSibling();   //가장 앞에 보이도록 마지막 자식으로 설정

        //드래그 가능한 오브젝트가 하나가 아닌 자식들을 가지고 있을 수도 때문에 CanvasGroup으로 총재
        //알파값을 0.6으로 설정하고,광선 충돌처리가 되지 않도록 한다
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// 현재 오브젝트를 드래그 중일 떄 매 프레임 호출
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        //현제 스크린상의 마우스 위치를 UI 위치로 설정 (UI가 마우스를 쫓아다디는 상태)
        rect.position = eventData.position;

    }

    /// <summary>
    /// 현재 오브젝트를 드래그 종료할 떄 1회 호출
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        //드래그를 시작하면 무보가 canvas로 설정되기 떄문
        //드래그를 종료할때 부모가 canvas이면 아이템 슬롯이 아닌 엉뚱한 곳에
        //드롭을 했다는 뜻이기 떄문에 드래그 작전에 소속되어 있던 아이템 슬롯으로 아이템 이동
        if(transform.parent == canvas)
        {
            //마지막에 소속되어있었던 previousParent의 자식으로 설정하고,해당 위치로 설정
            transform.SetParent(previousParent);
            rect.position = previousParent.GetComponent<RectTransform>().position;
        }
        //알파값을 1로 설정,광선 충돌처리 되게함
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
    }

}
