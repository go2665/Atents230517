using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_Equip : ItemData, IEquipable
{
    [Header("장비 아이템 데이터")]
    /// <summary>
    /// 아이템을 장비했을 때 플레이어 모델에 붙을 실제 장비 프리팹 
    /// </summary>
    public GameObject equipPrefab;
    
    /// <summary>
    /// 아이템이 장비될 위치를 알려주는 프로퍼티
    /// </summary>
    public virtual EquipType equipPart => EquipType.Weapon;

    /// <summary>
    /// 아이템을 장비하는 함수
    /// </summary>
    /// <param name="target">아이템을 장비할 대상</param>
    /// <param name="slot">아이템이 들어있는 슬롯</param>
    public void EquipItem(GameObject target, InvenSlot slot)
    {
        if(target != null)  // 대상이 있고
        {
            IEquipTarget equipTarget = target.GetComponent<IEquipTarget>(); // 대상이 아이템 장비를 할 수 있으면
            if (equipTarget != null)
            {
                equipTarget.EquipItem(equipPart, slot); // slot에 들어있는 아이템을 장비해라.
            }
        }
    }

    /// <summary>
    /// 아이템 장비를 해제하는 함수
    /// </summary>
    /// <param name="target">장비를 해제할 대상</param>
    /// <param name="slot">해제할 아이템이 들어있는 슬롯</param>
    public void UnEquipItem(GameObject target, InvenSlot slot)
    {
        if (target != null)     // 대상이 있고
        {
            IEquipTarget equipTarget = target.GetComponent<IEquipTarget>(); // 대상이 아이템을 장비할 수 있으면
            if (equipTarget != null)
            {
                equipTarget.UnEquipItem(equipPart); // 해당 파츠의 장비 해제를 해라.
            }
        }
    }

    /// <summary>
    /// 상황에 따라 아이템을 장비하거나 해제하는 함수
    /// </summary>
    /// <param name="target">아이템을 장비하거나 해제할 대상</param>
    /// <param name="slot">아이템이 들어있는 슬롯</param>
    public void ToggleEquip(GameObject target, InvenSlot slot)
    {
        if (target != null) // 대상이 있고
        {
            IEquipTarget equipTarget = target.GetComponent<IEquipTarget>();
            if (equipTarget != null)    // 대상이 아이템을 장비할 수 있으면
            {
                InvenSlot oldSlot = equipTarget[equipPart]; // 이 아이템이 장비되는 파츠를 확인
                if (oldSlot == null)        // oldSlot이 null이면 해당 파츠는 장비되어 있지 않은 상태
                {
                    EquipItem(target, slot);    // 장비되어 있지 않으면 이 아이템을 장비
                }
                else // oldSlot이 null이 아니면 해당 파츠는 장비되어 있는 상태
                {
                    UnEquipItem(target, oldSlot);   // 우선 기존 장비 해제
                    if(oldSlot != slot)             // 기존에 장비되어있던 것과 지금 파라메터로 받은 것이 다르면
                    {
                        EquipItem(target, slot);    // 새것을 장비
                    }
                }
            }
        }
    }
}
