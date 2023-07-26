using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    /// <summary>
    /// 이 UI가 보여줄 인벤토리 
    /// </summary>
    Inventory inven;

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
    DetailInfo detail;

    /// <summary>
    /// 이 인벤토리의 소유자를 확인하기 위한 프로퍼티
    /// </summary>
    public Player Owner => inven.Owner;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        slotsUI = child.GetComponentsInChildren<InvenSlotUI>();

        tempSlotUI = GetComponentInChildren<TempSlotUI>();

        detail = GetComponentInChildren<DetailInfo>();
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
        inven.MoveItem(tempSlotUI.Index, index);    // 임시 슬롯에서 도착 슬롯으로 아이템 옮기기
        if( tempSlotUI.InvenSlot.IsEmpty )          // 비었다면(같은 종류의 아이템일 때 일부만 들어가는 경우가 있을 수 있으므로)
        {
            tempSlotUI.Close();                     // 임시 슬롯 닫기
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
            // 아이템 사용, 장비 등등
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

    private void OnDetailPause(bool isPause)
    {
    }
}
