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

    /// <summary>
    /// 인벤토리에 아이템을 하나 추가하는 함수
    /// </summary>
    /// <param name="code">추가할 아이템 종류</param>
    /// <returns>true면 추가 성공, false면 추가 실패</returns>
    public bool AddItem(ItemCode code)
    {
        bool result = false;
        ItemData data = itemDataManager[code];

        InvenSlot sameDataSlot = FindSameItem(data);
        if(sameDataSlot != null)
        {
            // 같은 종류의 아이템이 있다.
            // 아이템 개수 1 증가시키기고 결과 받기
            result = sameDataSlot.IncreaseSlotItem(out _);  // 넘치는 개수가 의미 없어서 따로 받지 않음
        }
        else
        {
            // 같은 종류의 아이템이 없다.
            InvenSlot emptySlot = FindEmptySlot();
            if(emptySlot != null)
            {
                emptySlot.AssignSlotItem(data); // 빈슬롯이 있으면 아이템 하나 할당
                result = true;
            }
            else
            {
                // 비어있는 슬롯이 없다.
                Debug.Log("아이템 추가 실패 : 인벤토리가 가득 차있습니다.");
            }
        }

        return result;
    }

    /// <summary>
    /// 인벤토리의 특정 슬롯에 아이템을 하나 추가하는 함수
    /// </summary>
    /// <param name="code">추가할 아이템의 종류</param>
    /// <param name="slotIndex">아이템을 추가할 인덱스</param>
    /// <returns></returns>
    public bool AddItem(ItemCode code, uint slotIndex)
    {
        bool result = false;

        if( IsValidIndex(slotIndex) )   // 인덱스가 적절한지 확인
        {
            ItemData data = itemDataManager[code];  // 아이템 데이터 가져오기
            InvenSlot slot = slots[slotIndex];      // 아이템을 추가할 슬롯 가져오기
            if(slot.IsEmpty)
            {
                slot.AssignSlotItem(data);          // 슬롯이 비었으면 아이템 할당
            }
            else
            {
                // 슬롯에 아이템이 이미 있는데
                if(slot.ItemData == data)           // 아이템이 같은 종류이면
                {
                    result = slot.IncreaseSlotItem(out _);  // 아이템 증가
                }
                else
                {
                    // 다른 종류이면 실패
                    Debug.Log($"아이템 추가 실패 : 인벤토리 {slotIndex}번 슬롯에 다른 아이템이 들어있습니다.");
                }
            }
        }
        else
        {
            // 인덱스가 잘못된 경우도 실패
            Debug.Log($"아이템 추가 실패 : {slotIndex}번은 없는 인덱스입니다.");
        }

        return result;
    }

    /// <summary>
    /// 인벤토리에서 아이템을 일정 개수만큼 감소시키는 함수
    /// </summary>
    /// <param name="slotIndex">아이템을 감소시킬 슬롯의 인덱스</param>
    /// <param name="decreaseCount">감소시킬 개수</param>
    public void RemoveItem(uint slotIndex, uint decreaseCount = 1) 
    {
        if(IsValidIndex(slotIndex) )
        {
            InvenSlot invenSlot = slots[slotIndex];
            invenSlot.DecreaseSlotItem(decreaseCount);
        }
        else
        {
            Debug.Log($"아이템 감소 실패 : {slotIndex}는 없는 인덱스입니다.");
        }
    }

    /// <summary>
    /// 인벤토리에서 아이템을 삭제하는 함수
    /// </summary>
    /// <param name="slotIndex">아이템을 삭제할 슬롯의 인덱스</param>
    public void ClearSlot(uint slotIndex)
    {
        if (IsValidIndex(slotIndex))
        {
            InvenSlot invenSlot = slots[slotIndex];
            invenSlot.ClearSlotItem();
        }
        else
        {
            Debug.Log($"아이템 삭제 실패 : {slotIndex}는 없는 인덱스입니다.");
        }
    }

    /// <summary>
    /// 인벤토리를 전부 비우는 함수
    /// </summary>
    public void ClearInventory()
    {
        foreach(var slot in slots)
        {
            slot.ClearSlotItem();
        }
    }


    /// <summary>
    /// 인벤토리의 아이템을 from위치에서 to위치로 아이템을 이동시키는 함수
    /// </summary>
    /// <param name="from">위치 변경이 시작되는 인덱스</param>
    /// <param name="to">위치 변경이 끝나는 인덱스</param>
    public void MoveItem(uint from, uint to)
    {
        // from지점과 to지점이 다르고 from과 to가 모두 valid해야 한다.
        if( (from != to) &&  IsValidIndex(from) && IsValidIndex(to) )
        {
            InvenSlot fromSlot = (from == TempSlotIndex) ? TempSlot : slots[from];  // 임시 슬롯을 감안해서 삼항연산자로 처리
            if( !fromSlot.IsEmpty )
            {
                InvenSlot toSlot = (to == TempSlotIndex) ? TempSlot : slots[to];
                if( fromSlot.ItemData == toSlot.ItemData )  // 같은 종류의 아이템이면
                {
                    toSlot.IncreaseSlotItem(out uint overCount, fromSlot.ItemCount);    // 일단 from이 가진 개수만큼 to 감소 시도
                    fromSlot.DecreaseSlotItem(fromSlot.ItemCount - overCount);          // from에서 실제로 넘어간 숫자만큼 from 감소
                    Debug.Log($"{from}번 슬롯에서 {to}번 슬롯으로 아이템 합치기");
                }
                else
                {
                    // 다른 종류의 아이템이면 서로 스왑
                    ItemData tempData = fromSlot.ItemData;
                    uint tempCount = fromSlot.ItemCount;
                    fromSlot.AssignSlotItem(toSlot.ItemData, toSlot.ItemCount);
                    toSlot.AssignSlotItem(tempData, tempCount);
                    Debug.Log($"{from}번 슬롯과 {to}번 슬롯의 아이템 교체");
                }
            }
        }
    }

    /// <summary>
    /// 인벤토리 특정 슬롯에서 아이템을 일정량 덜어내어 임시 슬롯으로 보내는 함수
    /// </summary>
    /// <param name="slotIndex">아이템을 덜어낼 슬롯</param>
    /// <param name="count">덜어낼 개수</param>
    public void SplitItem(uint slotIndex, uint count)
    {
        if(IsValidIndex(slotIndex))
        {
            InvenSlot slot = slots[slotIndex];
            slot.DecreaseSlotItem(count);                   // 슬롯에서 덜어내고
            TempSlot.AssignSlotItem(slot.ItemData, count);  // 임시 슬롯에 할당하기
        }
    }

    /// <summary>
    /// 인벤토리를 정렬하는 함수 
    /// </summary>
    /// <param name="sortBy">정렬 기준</param>
    /// <param name="isAcending">true면 오름차순, false면 내림차순</param>
    public void SlotSorting(ItemSortBy sortBy, bool isAcending = true)
    {
        List<InvenSlot> beforeSlots = new List<InvenSlot>(SlotCount);
        foreach( InvenSlot slot in slots)
        {
            beforeSlots.Add(slot);
        }

        switch(sortBy)
        {
            case ItemSortBy.Code:
                beforeSlots.Sort((x, y) =>
                {
                    if (x.ItemData == null)
                        return 1;
                    if (y.ItemData == null)
                        return -1;
                    if( isAcending )
                    {
                        return x.ItemData.code.CompareTo(y.ItemData.code);
                    }
                    else
                    {
                        return y.ItemData.code.CompareTo(x.ItemData.code);
                    }
                });
                break;
            case ItemSortBy.Name:
                beforeSlots.Sort((x, y) =>
                {
                    if (x.ItemData == null)
                        return 1;
                    if (y.ItemData == null)
                        return -1;
                    if (isAcending)
                    {
                        return x.ItemData.itemName.CompareTo(y.ItemData.itemName);
                    }
                    else
                    {
                        return y.ItemData.itemName.CompareTo(x.ItemData.itemName);
                    }
                });
                break;
            case ItemSortBy.Price:
                beforeSlots.Sort((x, y) =>
                {
                    if (x.ItemData == null)
                        return 1;
                    if (y.ItemData == null)
                        return -1;
                    if (isAcending)
                    {
                        return x.ItemData.price.CompareTo(y.ItemData.price);
                    }
                    else
                    {
                        return y.ItemData.price.CompareTo(x.ItemData.price);
                    }
                });
                break;
        }

        List<(ItemData, uint)> sortedData = new List<(ItemData, uint)>(SlotCount);
        foreach(var slot in beforeSlots)
        {
            sortedData.Add((slot.ItemData, slot.ItemCount));
        }

        int index = 0;
        foreach(var data in sortedData)
        {
            slots[index].AssignSlotItem(data.Item1, data.Item2);
            index++;
        }
    }

    /// <summary>
    /// 인벤토리에 파라메터와 같은 종류의 아이템이 들어있는 슬롯(여유공간이 있는 슬롯)을 찾는 함수
    /// </summary>
    /// <param name="data">찾을 아이템 종류</param>
    /// <returns>같은 종류의 아이템이 인벤토리에 없으면 null, 있으면 null이 아닌 값</returns>
    InvenSlot FindSameItem(ItemData data)
    {
        InvenSlot findSlot = null;
        foreach(var slot in slots)  // 모든 슬롯을 다 돌면서
        {
            if (slot.ItemData == data && slot.ItemCount < slot.ItemData.maxStackCount)  // itemData가 같고 여유 공간이 있으면 찾았다.
            {
                findSlot = slot;
                break;
            }
        }

        return findSlot;
    }

    /// <summary>
    /// 인벤토리에서 비어있는 슬롯을 찾는 함수
    /// </summary>
    /// <returns>비어있는 슬롯(첫번째)</returns>
    InvenSlot FindEmptySlot()
    {
        InvenSlot findSlot = null;
        foreach (var slot in slots)     // 모든 슬롯을 다 돌면서
        {
            if(slot.IsEmpty)            // 비어있는 슬롯이 있으면 찾았다.
            {
                findSlot = slot;
                break;
            }
        }

        return findSlot;
    }

    /// <summary>
    /// 적절한 인덱스인지 확인하는 함수
    /// </summary>
    /// <param name="index">확인할 인덱스</param>
    /// <returns>true면 적절한 인덱스, false면 없는 인덱스</returns>
    bool IsValidIndex(uint index) => (index < SlotCount) || (index == TempSlotIndex);

    /// <summary>
    /// 테스트용 : 인벤토리 안의 내용을 콘솔창에 출력하는 함수
    /// </summary>
    public void PrintInventory()
    {
        // 예시
        // [ 루비(1/3), 사파이어(1/5), 에메랄드(2/5), (빈칸), (빈칸), (빈칸) ]

        string printText = "[ ";

        for(int i=0;i<SlotCount-1;i++)
        {
            if (!slots[i].IsEmpty)
            {
                printText += $"{slots[i].ItemData.itemName}({slots[i].ItemCount}/{slots[i].ItemData.maxStackCount})";
            }
            else
            {
                printText += "(빈칸)";
            }
            printText += ", ";
        }
        InvenSlot last = slots[SlotCount - 1];
        if(!last.IsEmpty)
        {
            printText += $"{last.ItemData.itemName}({last.ItemCount}/{last.ItemData.maxStackCount})";
        }
        else
        {
            printText += "(빈칸)";
        }
        printText += " ]";

        Debug.Log(printText);
    }
}
