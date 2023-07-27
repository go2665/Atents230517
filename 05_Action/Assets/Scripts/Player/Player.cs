using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Player : MonoBehaviour
{
    /// <summary>
    /// 플레이어의 인벤토리
    /// </summary>
    Inventory inven;

    /// <summary>
    /// 인벤토리 확인용 프로퍼티
    /// </summary>
    public Inventory Inventory => inven;

    /// <summary>
    /// 플레이어가 가지고 있는 금액
    /// </summary>
    int money = 0;

    /// <summary>
    /// 플레이어가 가지고 있는 금액 확인 및 설정용 프로퍼티
    /// </summary>
    public int Money
    {
        get => money;
        set
        {
            if(money != value)  // 금액이 변경되었을 때만
            {
                money = value;  // 수정하고
                onMoneyChange?.Invoke(money);   // 델리게이트로 알림
                Debug.Log($"Player Money : {money}");
            }
        }
    }

    /// <summary>
    /// 보유한 금액이 변경되었음을 알리는 델리게이트(파라메터:현재 보유한 금액)
    /// </summary>
    Action<int> onMoneyChange;


    /// <summary>
    /// 무기가 장착될 트랜스폼
    /// </summary>
    public Transform weaponParent;

    /// <summary>
    /// 방패가 장착될 트랜스폼
    /// </summary>
    public Transform shieldParent;

    /// <summary>
    /// 플레이어가 아이템을 줏을 수 있는 거리
    /// </summary>
    public float ItemPickupRange = 2.0f;

    /// <summary>
    /// 플레이어가 어떤 입력을 받았는지 처리하는 클래스
    /// </summary>
    PlayerInputController controller;

    private void Awake()
    {
        controller = GetComponent<PlayerInputController>();
        controller.onItemPickup = OnItemPickup;
    }

    private void Start()
    {
        inven = new Inventory(this);    // itemDataManager 설정 때문에 awake는 안됨
        if( GameManager.Inst.InvenUI != null )
        {
            GameManager.Inst.InvenUI.InitializeInventory( inven );  // 인벤토리와 인벤토리 UI연결
        }
    }

    /// <summary>
    /// 무기와 방패를 표시하거나 표시하지 않는 함수
    /// </summary>
    /// <param name="isShow"></param>
    public void ShowWeaponAndShield(bool isShow)
    {
        weaponParent.gameObject.SetActive(isShow);
        shieldParent.gameObject.SetActive(isShow);
    }

    /// <summary>
    /// 아이템 획득 처리를 하는 함수
    /// </summary>
    private void OnItemPickup()
    {
        // OverlapSphere를 이용해서 일정 반경 안에 Item이라는 레이어를 가진 컬라이더를 모두 찾기
        Collider[] itemColliders = Physics.OverlapSphere(transform.position, ItemPickupRange, LayerMask.GetMask("Item"));        
        foreach (Collider itemCollider in itemColliders)    // 찾은 모든 컬라이더에 대해
        {
            ItemObject item = itemCollider.GetComponent<ItemObject>();   // ItemObject 컴포넌트 찾기

            IConsumable consumable = item.ItemData as IConsumable;  // 즉시 소비가능한 아이템인지 확인용
            if( consumable != null )
            {
                // 즉시소비가능한 아이템이다.
                consumable.Consume(this.gameObject);
                Destroy(item.gameObject);
            }
            else if(inven.AddItem(item.ItemData.code))      // 즉시 소비가능한 아이템이 아니면 아이템 추가 시도
            {
                Destroy(item.gameObject);                   // 인벤토리에 아이템이 성공적으로 추가되면 삭제
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.blue;

        Handles.DrawWireDisc(transform.position, Vector3.up, ItemPickupRange);  // 아이템 획득범위(파란색)
    }
#endif
}
