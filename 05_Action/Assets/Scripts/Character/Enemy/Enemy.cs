using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Mathematics;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class Enemy : MonoBehaviour, IBattle, IHealth
{
    // 상태 머신
    // 상태 : 대기, 순찰, 추적, 공격, 사망
    protected enum EnemyState
    {
        Wait = 0,   // 대기
        Patrol,     // 순찰
        Chase,      // 추적
        Attack,     // 공격
        Dead        // 사망
    }

    /// <summary>
    /// 적의 현재 상태
    /// </summary>
    EnemyState state = EnemyState.Patrol;

    /// <summary>
    /// 적의 현재 상태를 확인하고 설정하는 프로퍼티
    /// </summary>
    protected EnemyState State
    {
        get => state;
        set
        {
            if (state != value)
            {
                state = value;
                switch (state)
                {
                    case EnemyState.Wait:
                        agent.isStopped = true;         // 에이전트 정지 시키기
                        agent.velocity = Vector3.zero;  // 남아있던 운동량 제거하기
                        animator.SetTrigger("Stop");
                        WaitTimer = waitTime;           // 대기 시간 초기화
                        onStateUpdate = Update_Wait;    // 대기 상태용 업데이트 함수 설정
                        break;
                    case EnemyState.Patrol:
                        agent.isStopped = false;                        // 에이전트 다시 켜기
                        agent.SetDestination(waypointTarget.position);  // 이동 명령
                        animator.SetTrigger("Move");
                        onStateUpdate = Update_Patrol;                  // 순찰 상태용 업데이트 함수 설정
                        break;
                    case EnemyState.Chase:
                        agent.isStopped = false;
                        animator.SetTrigger("Move");
                        onStateUpdate = Update_Chase;
                        break;
                    case EnemyState.Attack:
                        agent.isStopped = true;         // 에이전트 정지 시키기
                        agent.velocity = Vector3.zero;  // 남아있던 운동량 제거하기
                        animator.SetTrigger("Attack");
                        attackCoolTime = attackSpeed;
                        onStateUpdate = Update_Attack;
                        break;
                    case EnemyState.Dead:
                        agent.isStopped = true;         // 에이전트 정지 시키기
                        agent.velocity = Vector3.zero;  // 남아있던 운동량 제거하기
                        animator.SetTrigger("Die");     // 죽는 애니메이션 실행
                        onStateUpdate = Update_Dead;
                        break;
                    default:
                        break;
                }
                //Debug.Log($"State : {state}");
            }
        }
    }

    /// <summary>
    /// 대기 상태로 들어갔을 때 기다리는 시간
    /// </summary>
    public float waitTime = 1.0f;

    /// <summary>
    /// 대기 시간 측정용(0이 될 때까지 감소될 시간)
    /// </summary>
    float waitTimer = 1.0f;
    protected float WaitTimer
    {
        get => waitTimer;
        set
        {
            waitTimer = value;
            if( waitTimer < 0.0f )          // 대기 시간을 다 기다렸으면
            {
                State = EnemyState.Patrol;  // 순찰상태로 전환
            }
        }
    }

    /// <summary>
    /// 이동 속도
    /// </summary>
    public float moveSpeed = 3.0f;

    /// <summary>
    /// 적이 순찰할 웨이포인트
    /// </summary>
    public Waypoints waypoints;

    /// <summary>
    /// 적이 이동할 웨이포인트 지점을 가지는 트랜스폼
    /// </summary>
    protected Transform waypointTarget = null;    

    /// <summary>
    /// 원거리 시야 범위
    /// </summary>
    public float farSightRange = 10.0f;

    /// <summary>
    /// 시야각의 절반
    /// </summary>
    public float sightHalfAngle = 50.0f;

    /// <summary>
    /// 근접 시야 범위
    /// </summary>
    public float closeSightRange = 1.5f;

    /// <summary>
    /// 추적대상의 트랜스폼
    /// </summary>
    Transform chaseTarget = null;

    /// <summary>
    /// 공격 대상
    /// </summary>
    IBattle attackTarget = null;

    public float attackPower = 10.0f;
    public float AttackPower => attackPower;

    float attackSpeed = 1.0f;
    float attackCoolTime = 1.0f;

    public float defencePower = 3.0f;
    public float DefencePower => defencePower;

    float hp = 100.0f;
    public float HP 
    { 
        get => hp;
        set
        {
            hp = value;
            if( State != EnemyState.Dead && hp <= 0)
            {
                Die();
            }
            hp = Mathf.Clamp(hp, 0, maxHP);
            onHealthChange?.Invoke(hp/maxHP);
        }
    }

    public float maxHP = 100.0f;
    public float MaxHP => maxHP;

    public Action<float> onHealthChange { get; set; }
    public Action onDie { get; set; }

    public bool IsAlive => hp > 0;

    /// <summary>
    /// 상태별 업데이트 함수가 저장될 델리게이트(함수저장용)
    /// </summary>
    Action onStateUpdate;

    Animator animator;
    NavMeshAgent agent;
    SphereCollider bodyCollider;
    Rigidbody rigid;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        bodyCollider = GetComponent<SphereCollider>();
        rigid = GetComponent<Rigidbody>();

        AttackArea attackArea = GetComponentInChildren<AttackArea>();
        attackArea.onPlayerIn += (target) =>
        {
            if( State == EnemyState.Chase )     // 추적 상태이면
            {
                attackTarget = target;          // 공격 대상 지정하고
                State = EnemyState.Attack;      // 상태 변경하기
            }
        };
        attackArea.onPlayerOut += (target) =>
        {
            if( attackTarget == target )        // 공격 대상이 나가면
            {
                attackTarget = null;            // 공격 대상을 비우고
                if( State != EnemyState.Dead )  // 죽은 상태가 아니면
                {
                    State = EnemyState.Chase;   // 추적 상태로 변경
                }
            }
        };
    }

    private void Start()
    {
        agent.speed = moveSpeed;
        if( waypoints == null )
        {
            Debug.LogWarning("웨이포인트가 없습니다.");
            waypointTarget = transform;
        }
        else
        {
            waypointTarget = waypoints.Current;
        }

        State = EnemyState.Wait;
        animator.ResetTrigger("Stop");  // Wait 때문에 stop 트리거가 쌓이는 것 제거
    }

    private void Update()
    {
        onStateUpdate();
    }

    /// <summary>
    /// 대기 상태용 업데이트 함수
    /// </summary>
    void Update_Wait()
    {
        if( SearchPlayer() )
        {
            State = EnemyState.Chase;
        }
        else
        {
            WaitTimer -= Time.deltaTime;    // 대기 시간 감소(0이하기 되면 순살 상태로 변경)
        }
    }

    /// <summary>
    /// 순찰 상태용 업데이트 함수
    /// </summary>
    void Update_Patrol()
    {
        if (SearchPlayer())
        {
            State = EnemyState.Chase;
        }
        else 
        {
            // agent.pathPending : 경로 계산이 진행중인지 확인하는 프로퍼티. true면 경로 계산 중
            if ( !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) // 경로 계산이 끝났고 도착했는지 확인
            {
                waypointTarget = waypoints.MoveNext();  // 다음 이동 목적지 설정
                State = EnemyState.Wait;            // 대기 상태로 변경
            }
        }
    }

    /// <summary>
    /// 추적 상태용 업데이트 함수
    /// </summary>
    void Update_Chase()
    {
        if(SearchPlayer())
        {
            agent.SetDestination(chaseTarget.position);
        }
        else
        {
            State = EnemyState.Wait;
        }
    }

    /// <summary>
    /// 공격 상태용 업데이트 함수
    /// </summary>
    void Update_Attack()
    {
        attackCoolTime -= Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(attackTarget.transform.position - transform.position), 0.1f);
        if(attackCoolTime<0)
        {
            animator.SetTrigger("Attack");
            Attack(attackTarget);
        }
    }

    /// <summary>
    /// 사망 상태용 업데이트 함수
    /// </summary>
    void Update_Dead()
    {
    }

    /// <summary>
    /// 시야 범위 안에 플레이어가 있는지 찾는 함수
    /// </summary>
    /// <returns>true면 플레이어를 찾았다. false면 플레이어를 찾지 못했다.</returns>
    bool SearchPlayer()
    {
        bool result = false;
        chaseTarget = null;

        // 설정된 구안에 Player 레이어인 컬라이더 모두 찾기
        Collider[] colliders = Physics.OverlapSphere(transform.position, farSightRange, LayerMask.GetMask("Player"));
        if(colliders.Length > 0 )   // 배열 크기가 0보다 크면 플레이어가 1개 이상 있다는 소리
        {
            Vector3 playerPos = colliders[0].transform.position;    // 플레이어는 1개만 있으니 배열의 0번째는 플레이어, 플레이어 위치 가져옴
            Vector3 toPlayerDir = playerPos - transform.position;   // 적 위치에서 플레이어 위치로 가는 방향 백터 계산

            if(toPlayerDir.sqrMagnitude < closeSightRange * closeSightRange )   // 방향 백터의 길이를 이용해서 근접 범위 안에 있는지 확인
            {
                // 근접 시야 범위 안에 있다.
                chaseTarget = colliders[0].transform;
                result = true;
            }
            else
            {
                // 근접 시야 범위보다는 밖이므로 시야각 내부인지 확인
                if(IsInSightAngle(toPlayerDir))     // 플레이어로 향하는 방향벡터가 시야각 안에 있는지 확인
                {
                    if( IsSightClear(toPlayerDir) ) // 시야가 다른 문제에 의해 가려지는지 확인
                    {
                        chaseTarget = colliders[0].transform;
                        result = true;
                    }
                }
            }
        }

        return result;
    }  
    
    /// <summary>
    /// 시야각안에 대상이 있는지 없는지 확인하는 함수
    /// </summary>
    /// <param name="toTargetDirection">대상으로 향하는 방향 백터</param>
    /// <returns>시야각 안에 있으면 true, 없으면 false</returns>
    bool IsInSightAngle(Vector3 toTargetDirection)
    {
        float angle = Vector3.Angle(transform.forward, toTargetDirection);  // 적의 forward 벡터와 플레이어로 향하는 벡터의 사이각 계산
        return sightHalfAngle > angle;  // 사이각이 시야각의 절반보다 작으면 시야각 안에 있다.
    }

    /// <summary>
    /// 대상을 바라보는 시야가 다른 오브젝트에 의해 가려졌는지 아닌지 확인하는 함수
    /// </summary>
    /// <param name="toTargetDirection">대상으로 향하는 방향 백터</param>
    /// <returns></returns>
    bool IsSightClear(Vector3 toTargetDirection)
    {
        bool result = false;
        Ray ray = new(transform.position + transform.up * 0.5f, toTargetDirection); // 레이가 눈에서 나가는 것을 가정해서 만듬
        if( Physics.Raycast(ray, out RaycastHit hit, farSightRange))
        {
            if(hit.collider.CompareTag("Player"))
            {
                result = true;
            }
        }

        return result;
    }

    /// <summary>
    /// 공격함수
    /// </summary>
    /// <param name="target">내가 공격할 대상</param>
    public void Attack(IBattle target)
    {
        target.Defence(AttackPower);    // 대상에게 데미지를 주고
        attackCoolTime = attackSpeed;   // 쿨타임 초기화
    }

    /// <summary>
    /// 방어용 함수
    /// </summary>
    /// <param name="damage">내가 받은 데미지</param>
    public void Defence(float damage)
    {
        if( State != EnemyState.Dead )
        {
            animator.SetTrigger("Hit");
            // 데미지 공식 : 실제 입는 데미지 = 적 공격 데미지 - 방어력
            HP -= (damage - DefencePower);  // 데미지 적용
        }
    }

    /// <summary>
    /// 죽었을 때 실행될 함수
    /// </summary>
    public void Die()
    {
        State = EnemyState.Dead;
        StartCoroutine(Dead());
        onDie?.Invoke();
    }

    IEnumerator Dead()
    {
        // 바닥에 보일 소용돌이 이팩트 켜기
        // HP바 제거하기(안보이게 하는 것도 ok)

        // 죽는 애니메이션이 끝난 이후 (1.5초 이후)

        // 바닥 아래로 떨어트리기

        // 충분히 바닥아래로 떨어진 이후 (적당한 시간)

        // 슬라임 삭제하기
        // 이팩트도 삭제하기

        yield return null;
    }

    /// <summary>
    /// 적의 체력을 지속적으로 회복시키는 함수
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
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;          // 시간 카운팅
            HP += Time.deltaTime * regenPerSec;     // 초당 회복량만큼 증가
            yield return null;
        }
    }

    /// <summary>
    /// 적의 체력을 틱 단위로 증가 시키는 함수
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
        for (uint tickCount = 0; tickCount < totalTickCount; tickCount++)
        {
            HP += tickRegen;
            yield return wait;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        bool playerShow = false;
        playerShow = SearchPlayer();   // 시야 범위안에 플레이어가 들어왔는지 확인

        // 원거리 시야 범위는 녹색으로 표시(부채꼴로 그리기)
        Handles.color = playerShow ? Color.red : Color.green;
        Vector3 forward = transform.forward * farSightRange;
        Handles.DrawDottedLine(transform.position, transform.position + forward, 2.0f); // 중심선 그리고

        Quaternion q1 = Quaternion.AngleAxis(-sightHalfAngle, transform.up);            // 시야각의 절반만큼 회전시키는 회전 생성
        Quaternion q2 = Quaternion.AngleAxis(sightHalfAngle, transform.up);
        Handles.DrawLine(transform.position, transform.position + q1 * forward);        // forward에 위에서 만든 회전을 곱해서 회전된 위치 계산
        Handles.DrawLine(transform.position, transform.position + q2 * forward);
        Handles.DrawWireArc(transform.position, transform.up, q1 * forward, sightHalfAngle * 2.0f, farSightRange, 2.0f); // 호 그리기

        // 근거리 시야 범위는 노란색으로 표시
        Handles.color = playerShow ? Color.red : Color.yellow;
        Handles.DrawWireDisc(transform.position, transform.up, closeSightRange);

        // 단 플레이어가 시야 범위 안에 들어오면 빨간색
    }
#endif

}
