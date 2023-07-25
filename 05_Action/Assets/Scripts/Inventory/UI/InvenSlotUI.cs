using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 인벤토리의 슬롯용 UI.
/// </summary>
public class InvenSlotUI : SlotUI_Base, IDragHandler, IBeginDragHandler, IEndDragHandler,
    IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    /// <summary>
    /// 장비 여부를 표시하는 텍스트
    /// </summary>
    TextMeshProUGUI equippedText;

    /// <summary>
    /// 드래그 시작을 알리는 델리게이트. 파라메터는 드래그 시작한 슬롯의 인덱스
    /// </summary>
    public Action<uint> onDragBegin;

    /// <summary>
    /// 드래그 종료를 알리는 델리게이트. 파라메터는 드래그가 끝난 슬롯의 인덱스와 드래그가 슬롯에서 끝났는지 알리는 bool(슬롯에서 끝났으면 true)
    /// </summary>
    public Action<uint, bool> onDragEnd;

    /// <summary>
    /// 슬롯에 클릭이 있었다고 알리는 델리게이트. 파라메터는 클릭된 슬롯의 인덱스
    /// </summary>
    public Action<uint> onClick;

    /// <summary>
    /// 마우스 포인터가 슬롯 안으로 들어왔다고 알리는 델리게이트. 파라메터는 대상 슬롯의 인덱스
    /// </summary>
    public Action<uint> onPointerEnter;

    /// <summary>
    /// 마우스 포인터가 슬롯 밖으로 나갔다고 알리는 델리게이트. 파라메터는 대상 슬롯의 인덱스
    /// </summary>
    public Action<uint> onPointerExit;

    /// <summary>
    /// 마우스 포인터가 슬롯 위에서 움직인다고 알리는 델리게이트. 파라메터는 마우스 포인터의 스크린 좌표
    /// </summary>
    public Action<Vector2> onPointerMove;

    protected override void Awake()
    {
        base.Awake();
        Transform child = transform.GetChild(2);
        equippedText = child.GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// 이 슬롯 UI를 초기화하는 함수
    /// </summary>
    /// <param name="slot">이 UI와 연결될 슬롯</param>
    public override void InitializeSlot(InvenSlot slot)
    {
        // 델리게이트들 초기화
        onDragBegin = null; 
        onDragEnd = null;
        onClick = null;
        onPointerEnter = null;
        onPointerExit = null;
        onPointerMove = null;

        base.InitializeSlot(slot);
    }

    /// <summary>
    /// 슬롯UI가 갱신될 때 추가로 할 일
    /// </summary>
    protected override void OnRefresh()
    {
        // 장비 여부를 나타내는 글자 관련 추가로 처리
        if( InvenSlot.IsEquipped )
        {
            equippedText.color = Color.red;     // 장비중이면 빨간색
        }   
        else
        {
            equippedText.color = Color.clear;   // 장비중이 아니면 투명하게
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // OnBeginDrag와 OnEndDrag를 발동시키기 위해 추가한 것. 
        // 별도 실행하는 코드 없음
    }

    /// <summary>
    /// 드래그가 시작될 때 실행될 함수
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"드래그 시작 : {Index}번 슬롯");
        onDragBegin?.Invoke(Index);     // 델리게이트로 알람 보내기
    }

    /// <summary>
    /// 드래그가 종료될 때 실행될 함수
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그가 끝나는지점 확인
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;    // 마우스 있는 위치에 게임 오브젝트가 있는지 확인
        if( obj != null )
        {
            // 마우스 위치에 어떤 오브젝트가 있다.
            InvenSlotUI endSlot = obj.GetComponent<InvenSlotUI>();  // 마우스 위치에 있는 오브젝트가 슬롯UI인지 확인
            if( endSlot != null )
            {
                // 슬롯UI다.
                Debug.Log($"드래그 종료 : {endSlot.Index}번 슬롯");
                onDragEnd?.Invoke(endSlot.Index, true); // 끝난지점에 있는 슬롯의 인덱스와 정상적으로 끝났다고 알람 보내기
            }
            else
            {
                // 슬롯UI가 아니다.
                Debug.Log($"슬롯이 아닙니다.");           // (드래그 실패했을 때는 원래 위치로 되돌리는 것이 정상)
                onDragEnd?.Invoke(Index, false);        // 원래 드래그가 시작한 인덱스와 비정상적으로 끝났다고 알람 보내기
            }
        }
        else
        {
            // 마우스 위치에 아무런 오브젝트도 없다.
            Debug.Log("아무런 오브젝트도 없습니다.");
        }
    }

    /// <summary>
    /// 슬롯UI에 마우스가 클릭되었을 때 실행되는 함수
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke(Index);
    }

    /// <summary>
    /// 슬롯UI에 마우스가 들어왔을 때 실행되는 함수
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter?.Invoke(Index);
    }

    /// <summary>
    /// 슬롯UI에 마우스가 나갔을 때 실행되는 함수
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExit?.Invoke(Index);
    }

    /// <summary>
    /// 슬롯UI에서 마우스가 움직일 때 실행되는 함수
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerMove(PointerEventData eventData)
    {
        onPointerMove?.Invoke(eventData.position);
    }
}
