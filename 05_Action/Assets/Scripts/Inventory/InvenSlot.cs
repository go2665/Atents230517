using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenSlot
{
    /// <summary>
    /// 인벤토리에서의 인덱스
    /// </summary>
    uint slotIndex;

    /// <summary>
    /// 인벤토리에서의 인덱스를 확인하기 위한 프로퍼티
    /// </summary>
    public uint Index => slotIndex;

    /// <summary>
    /// 이 슬롯에 들어있는 아이템의 종류
    /// </summary>
    ItemData slotItemData = null;

    /// <summary>
    /// 이 슬롯에 들어있는 아이템의 종류를 확인하기 위한 프로퍼티(쓰기는 private)
    /// </summary>
    public ItemData ItemData
    {
        get => slotItemData;
        private set
        {
            if (slotItemData != value)      // 종류가 변경될 때만
            {
                slotItemData = value;       // 변경 작업 처리
                onSlotItemChange?.Invoke(); // 아이템 종류가 변경되었다고 알람 보내기
            }
        }
    }
    /// <summary>
    /// 슬롯에 들어있는 아이템의 종류, 개수, 장비 여부가 변경되었다고 알리는 델리게이트
    /// </summary>
    public Action onSlotItemChange;

    /// <summary>
    /// 슬롯에 아이템이 있는지 없는지 확인하는 프로퍼티(true면 비어있고, false면 아이템이 들어있다.)
    /// </summary>
    public bool IsEmpty => slotItemData == null;

    /// <summary>
    /// 이 슬롯에 들어있는 아이템 개수
    /// </summary>
    uint itemCount = 0;

    /// <summary>
    /// 아이템 개수를 확인하기 위한 프로퍼티(set은 private)
    /// </summary>
    public uint ItemCount
    {
        get => itemCount;
        private set
        {
            if(itemCount != value)              // 개수의 변경이 있을 때만 처리
            {
                itemCount = value;
                onSlotItemChange?.Invoke();     // 아이템 개수의 변경이 있었다고 알람 보내기
            }
        }
    }

    /// <summary>
    /// 이 슬롯의 아이템이 장비되었는지 여부
    /// </summary>
    bool isEquipped = false;

    /// <summary>
    /// 이 슬롯의 장비여부를 확인하기 위한 프로퍼티
    /// </summary>
    public bool IsEquipped
    {
        get => isEquipped;
        set
        {
            isEquipped = value;         
            onSlotItemChange?.Invoke(); // 무조건 변경되었다고 알림
        }
    }

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="index">이 슬롯의 인덱스(인벤토리에서 몇번째 슬롯인지)</param>
    public InvenSlot(uint index)
    {
        slotIndex = index;      // slotIndex는 이후로 절대 변하면 안된다.
        ItemCount = 0;
        IsEquipped = false;
    }

    /// <summary>
    /// 이 슬롯에 아이템을 설정하는 함수
    /// </summary>
    /// <param name="data">설정할 아이템 종류</param>
    /// <param name="count">설정할 아이템 개수(set 용도, 추가되는 것이 아님)</param>
    public void AssignSlotItem(ItemData data, uint count = 1, bool isEquipped = false)
    {
        if( data != null )
        {
            ItemData = data;
            ItemCount = count;
            IsEquipped = isEquipped;
            //Debug.Log($"인벤토리 {slotIndex}번 슬롯에 \"{ItemData.itemName}\" 아이템이 {ItemCount}개 설정");
        }
        else
        {
            ClearSlotItem();    // data가 null이면 해당 슬롯은 초기화
        }
    }

    /// <summary>
    /// 이 슬롯을 비우는 함수
    /// </summary>
    public void ClearSlotItem()
    {
        ItemData = null;
        ItemCount = 0;
        IsEquipped = false;
        //Debug.Log($"인벤토리 {slotIndex}번 슬롯을 비웁니다.");
    }

    /// <summary>
    /// 이 슬롯에 아이템 개수를 증가시키는 함수
    /// </summary>
    /// <param name="overCount">(출력용)추가하다가 넘친 개수</param>
    /// <param name="increaseCount">증가시킬 개수</param>
    /// <returns>성공여부(true면 안넘치고 다 추가 되서 성공, false면 넘쳤다)</returns>
    public bool IncreaseSlotItem(out uint overCount, uint increaseCount = 1)
    {
        bool result;
        int over;

        uint newCount = ItemCount + increaseCount;
        over = (int)newCount - (int)ItemData.maxStackCount;

        if( over > 0 )
        {
            // 넘쳤다.
            ItemCount = ItemData.maxStackCount;
            overCount = (uint)over;
            result = false;
            //Debug.Log($"인벤토리 {slotIndex}번 슬롯에 \"{ItemData.itemName}\" 아이템이 최대치까지 증가. 현재 {ItemCount}개. {over}개 넘침");
        }
        else
        {
            // 안넘쳤다.
            ItemCount = newCount;
            overCount = 0;
            result = true;
            //Debug.Log($"인벤토리 {slotIndex}번 슬롯에 \"{ItemData.itemName}\" 아이템이 {increaseCount}개 증가. 현재 {ItemCount}개.");
        }

        return result;
    }

    /// <summary>
    /// 이 슬롯에 아이템 개수 감소시키는 함수
    /// </summary>
    /// <param name="decreaseCount">감소시킬 아이템 개수</param>
    public void DecreaseSlotItem(uint decreaseCount = 1)
    {
        int newCount = (int)ItemCount - (int)decreaseCount;
        if(newCount < 1 )
        {
            // 슬롯이 완전히 비는 경우
            ClearSlotItem();
        }
        else
        {
            // 슬롯에 아이템이 남아있는 경우
            ItemCount = (uint)newCount;
            //Debug.Log($"인벤토리 {slotIndex}번 슬롯에 \"{ItemData.itemName}\" 아이템이 {decreaseCount}개 감소. 현재 {ItemCount}개.");
        }
    }

    /// <summary>
    /// 슬롯에 있는 아이템을 사용하는 함수
    /// </summary>
    /// <param name="target">아이템의 효과를 받을 대상</param>
    public void UseItem(GameObject target)
    {
        IUsable usable = ItemData as IUsable;   // IUsable을 상속 받았는지 확인
        if(usable != null)
        {
            if( usable.Use(target) )            // IUsable을 상속받았으면 사용 시도
            {
                DecreaseSlotItem();             // 성공적으로 사용했으면 개수 1개 감소
            }
        }
    }

    /// <summary>
    /// 슬롯에 있는 아이템을 장비하는 함수
    /// </summary>
    /// <param name="target">아이템을 장비할 대상</param>
    public void EquipItem(GameObject target)
    {
        IEquipable equip = ItemData as IEquipable;  // 장비 가능한 아이템이면
        if( equip != null )
        {
            equip.ToggleEquip(target, this);        // target에게 장비 시도
        }
    }
}
