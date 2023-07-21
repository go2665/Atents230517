using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 개념상 인벤토리(UI 없음)
/// </summary>
public class Inventory
{
    /// <summary>
    /// 인벤토리에 들어있는 인벤 슬롯의 기본 갯수
    /// </summary>
    public const int Default_Inventory_Size = 6;

    /// <summary>
    /// 임시슬롯용 인덱스
    /// </summary>
    public const uint TempSlotIndex = 999999999;

    /// <summary>
    /// 이 인벤토리에 들어있는 슬롯의 배열
    /// </summary>
    InvenSlot[] slots;

    /// <summary>
    /// 인벤토리 슬롯에 접근하기 위한 인덱서
    /// </summary>
    /// <param name="index">슬롯의 인덱스</param>
    /// <returns>슬롯</returns>
    public InvenSlot this[uint index] => slots[index];

    /// <summary>
    /// 인벤토리 슬롯의 갯수
    /// </summary>
    public int SlotCount => slots.Length;

    /// <summary>
    /// 임시 슬롯(드래그나 아이템 분리작업을 할 때 사용)
    /// </summary>
    InvenSlot tempSlot;
    public InvenSlot TempSlot => tempSlot;

    /// <summary>
    /// 아이템 데이터 메니저(아이템 종류별 데이터를 확인할 수 있다.)
    /// </summary>
    ItemDataManager itemDataManager;

    /// <summary>
    /// 인벤토리 소유자
    /// </summary>
    Player owner;
    public Player Owner => owner;

    /// <summary>
    /// 인벤토리 생성자
    /// </summary>
    /// <param name="owner">인벤토리 소유자</param>
    /// <param name="size">인벤토리의 크기</param>
    public Inventory(Player owner, uint size = Default_Inventory_Size)
    {
        slots = new InvenSlot[size];
        for(uint i=0;i<size;i++)
        {
            slots[i] = new InvenSlot(i);                // 슬롯 만들어서 저장
        }
        tempSlot = new InvenSlot(TempSlotIndex);
        itemDataManager = GameManager.Inst.ItemData;    // 아이템 데이터 메니저 캐싱
        this.owner = owner;                             // 소유자 기록
    }
}
