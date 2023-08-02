using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Weapon", menuName = "Scriptable Object/Item Data - Weapon", order = 8)]
public class ItemData_Weapon : ItemData_Equip
{
    [Header("무기 데이터")]

    /// <summary>
    /// 무기의 공격력
    /// </summary>
    public float attackPower = 30.0f;
}
