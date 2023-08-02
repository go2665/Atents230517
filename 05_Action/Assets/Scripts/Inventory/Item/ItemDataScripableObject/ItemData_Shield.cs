using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Shield", menuName = "Scriptable Object/Item Data - Shield", order = 9)]
public class ItemData_Shield : ItemData_Equip
{
    [Header("방패 데이터")]

    /// <summary>
    /// 방패의 방어력
    /// </summary>
    public float defencePower = 15.0f;

    public override EquipType equipPart => EquipType.Shield;
}
