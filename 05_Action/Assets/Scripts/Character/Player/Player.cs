using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Player : MonoBehaviour, IHealth, IMana, IEquipTarget, IBattle
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

    public float attackPower = 10.0f;
    public float AttackPower => attackPower;
    public float defencePower = 3.0f;
    public float DefencePower => defencePower;

    /// <summary>
    /// 장비 아이템의 부위별 장비 상태(장착한 아이템이 있는 슬롯을 가지고 있음)
    /// </summary>
    InvenSlot[] partsSlot;

    /// <summary>
    /// 장비 아이템의 부위별 슬롯 확인용 인덱서
    /// </summary>
    /// <param name="part">확인할 장비의 종류</param>
    /// <returns>null이면 장비가 안되어있음. null이 아니면 그 슬롯에 들어있는 아이템이 장비되어 있음</returns>
    public InvenSlot this[EquipType part] => partsSlot[(int)part];

    /// <summary>
    /// 보유한 금액이 변경되었음을 알리는 델리게이트(파라메터:현재 보유한 금액)
    /// </summary>
    public Action<int> onMoneyChange;

    /// <summary>
    /// 칼의 컬라이더 활성화/비활성화를 알리는 델리게이트
    /// </summary>
    Action<bool> onWeaponBladeEnable;

    /// <summary>
    /// 칼의 이팩트 활성화/비활성화를 알리는 델리게이트
    /// </summary>
    Action<bool> onWeaponEffectEnable;

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

    Animator animator;
    CinemachineVirtualCamera dieVCam;

    private void Awake()
    {
        controller = GetComponent<PlayerInputController>();
        controller.onItemPickup = OnItemPickup;
        controller.onLockOn = LockOnToggle;

        animator = GetComponent<Animator>();
        dieVCam = GetComponentInChildren<CinemachineVirtualCamera>();

        partsSlot = new InvenSlot[Enum.GetValues(typeof(EquipType)).Length];    // EquipType의 항목 개수만큼 배열 만들기

        lockOnEffect = transform.GetChild(6);
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
        animator.SetTrigger("Die");
        dieVCam.Follow = null;
        dieVCam.Priority = 20;
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

    public float lockOnRange = 5.0f;
    Transform lockOnTarget;     // 락온할 대상의 트랜스폼
    Transform lockOnEffect;     // 락온 표시용 이펙트의 트랜스폼
    public Transform LockOnTrarget
    {
        get => lockOnTarget;
        private set
        {
            if (lockOnTarget != value)
            {
                lockOnTarget = value;

                if (lockOnTarget != null)
                {
                    Debug.Log($"락온 대상 : {lockOnTarget.gameObject.name}");
                    Enemy enemy = lockOnTarget.GetComponent<Enemy>();
                    lockOnEffect.SetParent(enemy.transform);
                    lockOnEffect.transform.localPosition = Vector3.zero;
                    lockOnEffect.gameObject.SetActive(true);

                    enemy.onDie += () =>
                    {
                        lockOnTarget = null;
                        lockOnEffect.gameObject.SetActive(false);
                        lockOnEffect.SetParent(this.transform);
                        lockOnEffect.transform.localPosition = Vector3.zero;
                    };
                }
                else
                {
                    Debug.Log($"락온 대상 없음");
                    lockOnEffect.gameObject.SetActive(false);
                    lockOnEffect.SetParent(this.transform);
                    lockOnEffect.transform.localPosition = Vector3.zero;
                }
            }
        }
    }

    void LockOnToggle()
    {
        // 1. 특정 버튼을 눌렀을 때 실행된다.
        // 2. 락온 범위 안에 적이 있으면 가장 가까운 적을 선택한다.
        // 3. 락온 범위 안에 적이 없으면 락온은 해제된다.
        // 4. 락온한 적이 죽으면 락온은 해제된다.

        Collider[] enemies = Physics.OverlapSphere(transform.position, lockOnRange, LayerMask.GetMask("AttackTarget"));
        if(enemies.Length > 0 )
        {
            // 가장 가까운 적 찾기
            Transform nearest = null;
            float nearestDistance = float.MaxValue;
            foreach(var enemy in enemies) 
            {
                Vector3 dir = enemy.transform.position - transform.position;
                float distanceSqr = dir.sqrMagnitude;
                if( distanceSqr < nearestDistance )
                {
                    nearestDistance = distanceSqr;
                    nearest = enemy.transform;
                }
            }

            LockOnTrarget = nearest;
        }
        else
        {
            LockOnTrarget = null;
        }
    }

    /// <summary>
    /// 플레이어가 아이템을 장비하는 함수
    /// </summary>
    /// <param name="part">장비할 부위</param>
    /// <param name="slot">장비할 아이템이 들어있는 슬롯</param>
    public void EquipItem(EquipType part, InvenSlot slot)
    {
        ItemData_Equip equip = slot.ItemData as ItemData_Equip;     // 장비가능한 아이템인지 확인
        if (equip != null)  // 장비가 가능하면
        {
            Transform partParent = GetEquipParentTransform(part);           // 장비가 붙을 부모 트랜스폼 가져오고
            GameObject obj = Instantiate(equip.equipPrefab, partParent);    // 부모 트랜스폼의 자식으로 오브젝트 생성

            partsSlot[(int)part] = slot;    // 어느 슬롯의 아이템이 장비되었는지 기록
            slot.IsEquipped = true;         // 장비되었다고 알림

            if(part == EquipType.Weapon)
            {
                Weapon weapon = obj.GetComponent<Weapon>();
                onWeaponBladeEnable = weapon.BladeColliderEnable;
                onWeaponEffectEnable = weapon.EffectEnable;
            }
        }
    }

    /// <summary>
    /// 플레이어가 아이템 장비를 해제하는 함수
    /// </summary>
    /// <param name="part">아이템을 장비 해제할 부위</param>
    public void UnEquipItem(EquipType part)
    {
        Transform partParent = GetEquipParentTransform(part);   // 판단기준 : 파츠 부모가 자식을 가지고 있으면 장비중
        while(partParent.childCount > 0)        // 파츠부모가 자식을 가지고 있으면 모두 제거
        {
            Transform child = partParent.GetChild(0);
            child.SetParent(null);      
            Destroy(child.gameObject);          // 자식 삭제
        }

        partsSlot[(int)part].IsEquipped = false;    // 장비 해제되었다고 알림
        partsSlot[(int)part] = null;                // 파츠 기록 초기화

        if( part == EquipType.Weapon)
        {
            onWeaponBladeEnable = null;
            onWeaponEffectEnable = null;
        }
    }

    /// <summary>
    /// 장비가 붙을 부모 트랜스폼 찾아주는 함수
    /// </summary>
    /// <param name="part">찾을 파츠</param>
    /// <returns>장비의 부모 트랜스폼</returns>
    public Transform GetEquipParentTransform(EquipType part)
    {
        Transform result = null;
        switch(part)
        {
            case EquipType.Weapon:
                result = weaponParent;
                break;
            case EquipType.Shield: 
                result = shieldParent; 
                break;
        }
        return result;
    }

    public void WeaponEffectEnable(bool enable)
    {
        onWeaponEffectEnable?.Invoke(enable);
    }

    public void WeaponBladeEnable()
    {
        onWeaponBladeEnable?.Invoke(true);
    }

    public void WeaponBladeDisable()
    {
        onWeaponBladeEnable?.Invoke(false);
    }

    /// <summary>
    /// 파츠별로 어떤 슬롯에 아이템이 사용되고 있는지를 설정하는 함수
    /// </summary>
    /// <param name="parts">아이템 파츠 종류</param>
    /// <param name="slot">아이템이 들어있는 슬롯</param>
    public void SetPartsSlot(EquipType parts, InvenSlot slot)
    {
        partsSlot[(int)parts] = slot;
    }

    /// <summary>
    /// 공격함수
    /// </summary>
    /// <param name="target">내가 공격할 대상</param>
    public void Attack(IBattle target)
    {
        target.Defence(AttackPower);    // 대상에게 데미지를 주기
    }

    /// <summary>
    /// 방어용 함수
    /// </summary>
    /// <param name="damage">내가 받은 데미지</param>
    public void Defence(float damage)
    {
        if (IsAlive)
        {
            animator.SetBool("SkillEnd", true);
            WeaponBladeDisable();
            animator.SetTrigger("Hit");
            // 데미지 공식 : 실제 입는 데미지 = 적 공격 데미지 - 방어력
            HP -= Mathf.Max(0, damage - DefencePower);  // 데미지 적용
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // 아이템 획득 범위
        Handles.color = Color.blue;
        Handles.DrawWireDisc(transform.position, Vector3.up, ItemPickupRange);  // 아이템 획득범위(파란색)

        // 락온 범위
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, lockOnRange, 2.0f);
    }

#endif
}
