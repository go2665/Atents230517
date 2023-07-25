using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InvenSlotUI : SlotUI_Base, IDragHandler, IBeginDragHandler, IEndDragHandler,
    IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    TextMeshProUGUI equippedText;

    public Action<uint> onDragBegin;
    public Action<uint, bool> onDragEnd;
    public Action<uint> onClick;
    public Action<uint> onPointerEnter;
    public Action<uint> onPointerExit;
    public Action<Vector2> onPointerMove;

    protected override void Awake()
    {
        base.Awake();
        Transform child = transform.GetChild(2);
        equippedText = child.GetComponent<TextMeshProUGUI>();
    }

    public override void InitializeSlot(InvenSlot slot)
    {
        onDragBegin = null; 
        onDragEnd = null;
        onClick = null;
        onPointerEnter = null;
        onPointerExit = null;
        onPointerMove = null;

        base.InitializeSlot(slot);
    }

    protected override void OnRefresh()
    {
        if( InvenSlot.IsEquipped )
        {
            equippedText.color = Color.red;
        }
        else
        {
            equippedText.color = Color.clear;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // OnBeginDrag와 OnEndDrag를 발동시키기 위해 추가한 것. 
        // 별도 실행하는 코드 없음
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"드래그 시작 : {Index}번 슬롯");
        onDragBegin?.Invoke(Index);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;
        if( obj != null )
        {
            InvenSlotUI endSlot = obj.GetComponent<InvenSlotUI>();
            if( endSlot != null )
            {
                Debug.Log($"드래그 종료 : {endSlot.Index}번 슬롯");
                onDragEnd?.Invoke(endSlot.Index, true);
            }
            else
            {
                Debug.Log($"슬롯이 아닙니다.");
                onDragEnd?.Invoke(Index, false);
            }
        }
        else
        {
            Debug.Log("아무런 오브젝트가 없습니다.");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke(Index);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter?.Invoke(Index);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExit?.Invoke(Index);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        onPointerMove?.Invoke(eventData.position);
    }
}
