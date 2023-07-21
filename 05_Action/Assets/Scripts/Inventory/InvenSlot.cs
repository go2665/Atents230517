using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenSlot
{
    uint slotIndex;
    public uint Index => slotIndex;

    ItemData slotItemData = null;
    public ItemData ItemData
    {
        get => slotItemData;
        private set
        {
            if (slotItemData != value)
            {
                slotItemData = value;
                onSlotItemChange?.Invoke();
            }
        }
    }
    public Action onSlotItemChange;

    public bool IsEmpty => slotItemData == null;

    uint itemCount = 0;
    public uint ItemCount
    {
        get => itemCount;
        private set
        {
            if(itemCount != value)
            {
                itemCount = value;
                onSlotItemChange?.Invoke();
            }
        }
    }

    bool isEquipped = false;
    public bool IsEquipped
    {
        get => isEquipped;
        set
        {
            IsEquipped = value;
            onSlotItemChange?.Invoke();
        }
    }

    public InvenSlot(uint index)
    {
        slotIndex = index;
        ItemCount = 0;
        IsEquipped = false;
    }

    public void AssignSlotItem(ItemData itemData, uint count = 1)
    {
    }

    public void ClearSlotItem()
    {
    }

    public bool IncreaseSlotItem(out uint overCount, uint increaseCount = 1)
    {
        bool result;
        
        // 나중에 제거할 코드
        result = false;
        overCount = 1;
        // -----------------

        return result;
    }

    public void DecreaseSlotItem(uint decreaseCount = 1)
    {
    }

    public void UseItem(GameObject target)
    {
    }

    public void EquipItem(GameObject target)
    {
    }
}
