using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템을 장비할 수 있는 대상이 가지고 있을 인터페이스
/// </summary>
public interface IEquipTarget
{
    /// <summary>
    /// 장비 아이템의 부위별 슬롯 확인용 인덱서
    /// </summary>
    /// <param name="part">확인할 파츠</param>
    /// <returns>null이면 장비되어있지 않다. null이 아니면 그 슬롯에 들어있는 아이템이 장비된 상태</returns>
    InvenSlot this[EquipType part] { get; }

    /// <summary>
    /// 아이템을 장비하는 함수
    /// </summary>
    /// <param name="part">장비할 부위</param>
    /// <param name="slot">장비할 아이템이 들어있는 슬롯</param>
    void EquipItem(EquipType part, InvenSlot slot);
    
    /// <summary>
    /// 아이템을 장비 해제하는 함수
    /// </summary>
    /// <param name="part">아이템을 해제할 파츠</param>
    void UnEquipItem(EquipType part);

    /// <summary>
    /// 장비될 아이템이 자식으로 들어갈 트랜스폼을 돌려주는 함수
    /// </summary>
    /// <param name="part">장비될 부위</param>
    /// <returns>장비될 부위의 부모트랜스폼</returns>
    Transform GetEquipParentTransform(EquipType part);
}
