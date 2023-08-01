using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Player : MonoBehaviour, IHealth, IMana
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
    /// 현재 HP
    /// </summary>
    float hp = 100.0f;
    public float HP 
    { 
        get => hp;
        set
        {
            if( IsAlive )       // 살아있을 때만 HP 변경
            {
                hp = value;
                if( hp <= 0 )   // hp가 0 이하면 사망
                {
                    Die();
                }
                hp = Mathf.Clamp(hp, 0, MaxHP);     // HP는 항상 0~최대치
                onHealthChange?.Invoke(hp/MaxHP);   // HP 변화 알리기
            }
        }
    }

    /// <summary>
    /// 최대 HP
    /// </summary>
    float maxHP = 100.0f;
    public float MaxHP => maxHP;

    /// <summary>
    /// HP가 변경되었을 때 실행될 델리게이트
    /// </summary>
    public Action<float> onHealthChange { get; set; }

    /// <summary>
    /// 플레이어가 사망했을 때 실행될 델리게이트
    /// </summary>
    public Action onDie { get; set; }

    /// <summary>
    /// 플레이어 생존 여부
    /// </summary>
    public bool IsAlive => hp > 0;

    /// <summary>
    /// 플레이어의 현재 MP
    /// </summary>
    float mp = 150.0f;
    public float MP 
    { 
        get => mp;
        set
        {
            if (IsAlive)       // 살아있을 때만 MP 변경
            {
                mp = Mathf.Clamp(value, 0, MaxMP);  // MP는 항상 0~최대치
                onManaChange?.Invoke(mp / MaxMP);   // MP 변화 알리기
            }
        }
    }

    /// <summary>
    /// 플레이어의 최대 MP
    /// </summary>
    float maxMP = 150.0f;
    public float MaxMP => maxMP;

    /// <summary>
    /// 마나가 변경되었을 때 실행될 델리게이트
    /// </summary>
    public Action<float> onManaChange { get; set; }

    /// <summary>
    /// 보유한 금액이 변경되었음을 알리는 델리게이트(파라메터:현재 보유한 금액)
    /// </summary>
    public Action<int> onMoneyChange;


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

    public void Die()
    {
        onDie?.Invoke();
        Debug.Log("플레이어 사망");
    }

    /// <summary>
    /// 플레이어의 체력을 지속적으로 회복시키는 함수
    /// </summary>
    /// <param name="totalRegen">전체 회복량</param>
    /// <param name="duration">전체 회복 시간</param>
    public void HealthRegenetate(float totalRegen, float duration)
    {
        StartCoroutine(HealthRegetateCoroutine(totalRegen, duration));
    }

    IEnumerator HealthRegetateCoroutine(float totalRegen, float duration)
    {
        float regenPerSec = totalRegen / duration;  // 초당 회복량 계산
        float timeElapsed = 0.0f;
        while(timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;          // 시간 카운팅
            HP += Time.deltaTime * regenPerSec;     // 초당 회복량만큼 증가
            yield return null;
        }
    }

    /// <summary>
    /// 플레이어의 체력을 틱 단위로 증가 시키는 함수
    /// </summary>
    /// <param name="tickRegen">틱당 회복량</param>
    /// <param name="tickTime">한 틱당 시간 간격</param>
    /// <param name="totalTickCount">전체 틱 수</param>
    public void HealthRegenerateByTick(float tickRegen, float tickTime, uint totalTickCount)
    {
        StartCoroutine(HealthRegenerateByTickCoroutine(tickRegen, tickTime, totalTickCount));
    }

    IEnumerator HealthRegenerateByTickCoroutine(float tickRegen, float tickTime, uint totalTickCount)
    {
        WaitForSeconds wait = new WaitForSeconds(tickTime);
        for(uint tickCount = 0; tickCount < totalTickCount; tickCount++)
        {
            HP += tickRegen;
            yield return wait;
        }
    }

    public void ManaRegenetate(float totalRegen, float duration)
    {
        StartCoroutine(ManaRegetateCoroutine(totalRegen, duration));
    }

    IEnumerator ManaRegetateCoroutine(float totalRegen, float duration)
    {
        float regenPerSec = totalRegen / duration;  // 초당 회복량 계산
        float timeElapsed = 0.0f;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;          // 시간 카운팅
            MP += Time.deltaTime * regenPerSec;     // 초당 회복량만큼 증가
            yield return null;
        }
    }

    public void ManaRegenerateByTick(float tickRegen, float tickTime, uint totalTickCount)
    {
        StartCoroutine(ManaRegenerateByTickCoroutine(tickRegen, tickTime, totalTickCount));
    }

    IEnumerator ManaRegenerateByTickCoroutine(float tickRegen, float tickTime, uint totalTickCount)
    {
        WaitForSeconds wait = new WaitForSeconds(tickTime);
        for (uint tickCount = 0; tickCount < totalTickCount; tickCount++)
        {
            MP += tickRegen;
            yield return wait;
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
