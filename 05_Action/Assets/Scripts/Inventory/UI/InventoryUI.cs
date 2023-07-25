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
    /// 이 인벤토리의 소유자를 확인하기 위한 프로퍼티
    /// </summary>
    public Player Owner => inven.Owner;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        slotsUI = child.GetComponentsInChildren<InvenSlotUI>();

        tempSlotUI = GetComponentInChildren<TempSlotUI>();
    }

    public void InitializeInventory(Inventory playerInven)
    {
        inven = playerInven;

        // 슬롯 초기화
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
    }

    private void OnItemMoveBegin(uint index)
    {
        throw new NotImplementedException();
    }

    private void OnItemMoveEnd(uint index, bool isSuccess)
    {
        throw new NotImplementedException();
    }

    private void OnSlotClick(uint index)
    {
        throw new NotImplementedException();
    }

    private void OnItemDetailOn(uint index)
    {
        throw new NotImplementedException();
    }

    private void OnItemDetailOff(uint index)
    {
        throw new NotImplementedException();
    }

    private void OnSlotPointerMove(Vector2 screenPos)
    {
        throw new NotImplementedException();
    }

    private void OnDetailPause(bool isPause)
    {
        throw new NotImplementedException();
    }
}
