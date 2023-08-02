using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipable
{
    /// <summary>
    /// 이 아이템이 장착될 부위
    /// </summary>
    EquipType equipPart { get; }

    /// <summary>
    /// 아이템을 장착하는 함수
    /// </summary>
    /// <param name="target">장착받을 대상</param>
    /// <param name="slot">장착할 아이템이 있는 슬롯</param>
    void EquipItem(GameObject target, InvenSlot slot);

    /// <summary>
    /// 아이템을 해제하는 함수
    /// </summary>
    /// <param name="target">장착 해제할 대상</param>
    /// <param name="slot">해제할 아이템이 있는 슬롯</param>
    void UnEquipItem(GameObject target, InvenSlot slot);

    /// <summary>
    /// 상황에 따라 아이템을 장착하고 해제하는 함수
    /// </summary>
    /// <param name="target">장착 및 해제 대상</param>
    /// <param name="slot">아이템이 있는 슬롯</param>
    void ToggleEquip(GameObject target, InvenSlot slot);
}
