using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    /// <summary>
    /// 이 UI가 보여줄 인벤토리 
    /// </summary>
    Inventory inven;

    /// <summary>
    /// 쉬프트키가 눌려졌는지 여부를 기록하는 변수
    /// </summary>
    bool isShiftPress = false;

    /// <summary>
    /// 이 인벤토리가 가지고 있는 모든 슬롯의 UI
    /// </summary>
    InvenSlotUI[] slotsUI;

    /// <summary>
    /// 아이템 이동이나 분리할 때 사용할 임시 슬롯 UI
    /// </summary>
    TempSlotUI tempSlotUI;

    /// <summary>
    /// 아이템의 상세정보를 표시하는 패널
    /// </summary>
    DetailInfoUI detail;

    /// <summary>
    /// 아이탬을 덜어내는 패널
    /// </summary>
    ItemSpliterUI spliter;

    /// <summary>
    /// 소유자가 가지고 있는 금액을 보여주는 패널
    /// </summary>
    MoneyPanel moneyPanel;

    /// <summary>
    /// 이 인벤토리의 소유자를 확인하기 위한 프로퍼티
    /// </summary>
    public Player Owner => inven.Owner;

    /// <summary>
    /// 인풋 액션
    /// </summary>
    PlayerInputActions inputActions;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        slotsUI = child.GetComponentsInChildren<InvenSlotUI>();

        tempSlotUI = GetComponentInChildren<TempSlotUI>();

        detail = GetComponentInChildren<DetailInfoUI>();

        spliter = GetComponentInChildren<ItemSpliterUI>();
        spliter.onOkClick += OnSpliterOk;

        moneyPanel = GetComponentInChildren<MoneyPanel>();

        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.Shift.performed += OnShiftPress;
        inputActions.UI.Shift.canceled += OnShiftPress;
        inputActions.UI.Click.canceled += OnItemDrop;
    }

    private void OnDisable()
    {
        inputActions.UI.Click.canceled -= OnItemDrop;
        inputActions.UI.Shift.performed -= OnShiftPress;
        inputActions.UI.Shift.canceled -= OnShiftPress;
        inputActions.UI.Disable();
    }

    /// <summary>
    /// 인벤토리 UI 초기화 함수
    /// </summary>
    /// <param name="playerInven">이 UI와 연결될 인벤토리</param>
    public void InitializeInventory(Inventory playerInven)
    {
        inven = playerInven;

        // 슬롯 초기화(초기화 함수 실행 및 델리게이트 연결하기)
        for(uint i=0;i<slotsUI.Length;i++)
        {
            slotsUI[i].InitializeSlot(inven[i]);
            slotsUI[i].onDragBegin += OnItemMoveBegin;
            slotsUI[i].onDragEnd += OnItemMoveEnd;
            slotsUI[i].onClick += OnSlotClick;
            slotsUI[i].onPointerEnter += OnItemDetailOn;
            slotsUI[i].onPointerExit += OnItemDetailOff;
            slotsUI[i].onPointerMove += OnSlotPointerMove;
        }

        // 임시 슬롯 초기화
        tempSlotUI.InitializeSlot(inven.TempSlot);
        tempSlotUI.onTempSlotOpenClose += OnDetailPause;

        // 상세 정보창 닫아 놓기
        detail.Close();

        // 아이템 분리창 닫아 놓기
        spliter.onCancel += () => detail.IsPause = false;   // 캔슬버턴 누르면 상세정보창 일시정지 해제
        spliter.Close();

        // 오너와 머니 패널 연결하기
        Owner.onMoneyChange += moneyPanel.Refresh;
        moneyPanel.Refresh(Owner.Money);
    }

    /// <summary>
    /// 슬롯UI에서 드래그가 시작되면 실행될 함수
    /// </summary>
    /// <param name="index">드래그가 시작된 슬롯의 인덱스</param>
    private void OnItemMoveBegin(uint index)
    {
        inven.MoveItem(index, tempSlotUI.Index);    // 시작 슬롯에서 임시 슬롯으로 아이템 옮기기
        tempSlotUI.Open();                          // 임시 슬롯 열기
    }

    /// <summary>
    /// 슬롯UI에서 드래그가 끝났을 때 실행될 함수
    /// </summary>
    /// <param name="index">드래그가 끝난 슬롯의 인덱스</param>
    /// <param name="isSuccess">드래그가 성공적인지 여부</param>
    private void OnItemMoveEnd(uint index, bool isSuccess)
    {
        uint finalIndex = index;                        // 실제 사용할 인덱스(기본적으로는 파라메터로 받은 인덱스)
        if( !isSuccess )
        {
            if(inven.FindEmpySlotIndex(out uint emptyIndex))    // 드래그 실패했을 경우는 빈슬롯의 인덱스를 사용
            {
                finalIndex = emptyIndex;
            }
        }

        inven.MoveItem(tempSlotUI.Index, finalIndex);   // 임시 슬롯에서 도착 슬롯으로 아이템 옮기기
        if (tempSlotUI.InvenSlot.IsEmpty)               // 비었다면(같은 종류의 아이템일 때 일부만 들어가는 경우가 있을 수 있으므로)
        {
            tempSlotUI.Close();                         // 임시 슬롯 닫기
        }

        if( isSuccess )
        {
            detail.Open(inven[finalIndex].ItemData);    // 드래그가 성공적으로 끝났으면 상세 정보창도 열기
        }

    }

    /// <summary>
    /// 슬롯UI에 마우스가 클릭이 되었을 때 실행될 함수
    /// </summary>
    /// <param name="index">클릭된 슬롯의 인덱스</param>
    private void OnSlotClick(uint index)
    {
        if( tempSlotUI.InvenSlot.IsEmpty )
        {
            if( isShiftPress )
            {
                OnSpliterOpen(index);   // 임시 슬롯에 아이템이 없는데 쉬프트가 눌러진체로 슬롯을 클릭했을 때 아이템 분리창을 열어라
            }
            else
            {
                // 아이템 사용, 장비 등등
            }
        }
        else
        {
            // 임시 슬롯에 아이템이 있을 때 클릭이 되었으면
            OnItemMoveEnd(index, true); // 클릭된 슬롯으로 아이템 이동
        }
    }

    /// <summary>
    /// 마우스 포인터가 슬롯위에 올라왔을 때 실행되는 함수
    /// </summary>
    /// <param name="index">올라간 슬롯의 인덱스</param>
    private void OnItemDetailOn(uint index)
    {
        detail.Open(slotsUI[index].InvenSlot.ItemData); // 상세정보창 열기
    }

    /// <summary>
    /// 마우스 포인터가 슬롯위에서 나갔을 때 실행되는 함수
    /// </summary>
    /// <param name="index">나간 슬롯의 인덱스</param>
    private void OnItemDetailOff(uint index)
    {
        detail.Close(); // 상세정보창 닫기
    }

    /// <summary>
    /// 마우스 포인터가 슬롯위에서 움직일 때 실행되는 함수
    /// </summary>
    /// <param name="index">슬롯의 인덱스</param>
    private void OnSlotPointerMove(Vector2 screenPos)
    {
        detail.MovePosition(screenPos);
    }

    /// <summary>
    /// 임시 슬롯이 열리면 상세정보창을 일시 정지하고, 닫히면 일시 정지를 푸는 함수
    /// </summary>
    /// <param name="isPause">true면 일시 정지, false 해제</param>
    private void OnDetailPause(bool isPause)
    {
        detail.IsPause = isPause;
    }

    /// <summary>
    /// 아이템 분리창을 여는 함수
    /// </summary>
    /// <param name="index">아이템을 분리할 슬롯의 인덱스</param>
    private void OnSpliterOpen(uint index)
    {
        InvenSlotUI target = slotsUI[index];
        spliter.transform.position = target.transform.position + Vector3.up * 100;
        spliter.Open(target.InvenSlot);
        detail.IsPause = true;
    }

    /// <summary>
    /// 아이템 분리창에서 OK 버튼이 눌러졌을 때 실행될 함수
    /// </summary>
    /// <param name="index">아이템이 분리될 슬롯</param>
    /// <param name="count">분리된 개수</param>
    private void OnSpliterOk(uint index, uint count)
    {
        inven.SplitItem(index, count);
        tempSlotUI.Open();
    }

    /// <summary>
    /// 쉬프트키가 눌려지거나 때졌을 때 실행될 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnShiftPress(InputAction.CallbackContext context)
    {
        isShiftPress = !context.canceled;   // 쉬프트키 상황 기록
    }


    /// <summary>
    /// 마우스 클릭이 떨어졌을 때 실행되는 함수(아이템 드랍용)
    /// </summary>
    private void OnItemDrop(InputAction.CallbackContext _)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        RectTransform rectTransform = (RectTransform)transform;
        Vector2 diff = screenPos - (Vector2)rectTransform.position;

        if( !rectTransform.rect.Contains(diff) )
        {
            // 인벤토리 영역 밖이면
            tempSlotUI.OnDrop(screenPos);
        }
    }
}
