using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class Enemy : MonoBehaviour
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
                        WaitTimer = waitTime;           // 대기 시간 초기화
                        onStateUpdate = Update_Wait;    // 대기 상태용 업데이트 함수 설정
                        break;
                    case EnemyState.Patrol:
                        agent.SetDestination(moveTarget.position);  // 이동 명령
                        onStateUpdate = Update_Patrol;              // 순찰 상태용 업데이트 함수 설정
                        break;
                    case EnemyState.Chase:
                        onStateUpdate = Update_Chase;
                        break;
                    case EnemyState.Attack:
                        onStateUpdate = Update_Attack;
                        break;
                    case EnemyState.Dead:
                        onStateUpdate = Update_Dead;
                        break;
                    default:
                        break;
                }
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
    /// 적이 이동할 목적지를 가지는 트랜스폼(웨이포인트 지점 or 플레이어 지점)
    /// </summary>
    protected Transform moveTarget;    

    /// <summary>
    /// 원거리 시야 범위
    /// </summary>
    public float farSightRange;

    /// <summary>
    /// 시야각의 절반
    /// </summary>
    public float sightHalfAngle;

    /// <summary>
    /// 근접 시야 범위
    /// </summary>
    public float closeSightRange;

    ///// <summary>
    ///// 추적대상의 트랜스폼
    ///// </summary>
    //Transform chaseTarget;

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
    }

    private void Start()
    {
        agent.speed = moveSpeed;
        if( waypoints == null )
        {
            Debug.LogWarning("웨이포인트가 없습니다.");
            moveTarget = transform;
        }
        else
        {
            moveTarget = waypoints.Current;
        }

        State = EnemyState.Wait;
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
        WaitTimer -= Time.deltaTime;    // 대기 시간 감소(0이하기 되면 순살 상태로 변경)
    }

    /// <summary>
    /// 순찰 상태용 업데이트 함수
    /// </summary>
    void Update_Patrol()
    {
        if( agent.remainingDistance <= agent.stoppingDistance ) // 도착했는지 확인
        {
            moveTarget = waypoints.MoveNext();  // 다음 이동 목적지 설정
            State = EnemyState.Wait;            // 대기 상태로 변경
        }
    }

    /// <summary>
    /// 추적 상태용 업데이트 함수
    /// </summary>
    void Update_Chase()
    {
    }

    /// <summary>
    /// 공격 상태용 업데이트 함수
    /// </summary>
    void Update_Attack()
    {
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

        //Physics.OverlapSphere()

        // farSightRange 거리 안에 있는지 확인
        // 안에 있으면 시야각 안에 있는지 확인
        // 시야가 막혔는지 안막혔는지 확인

        return result;
    }  
    
    /// <summary>
    /// 시야각안에 대상이 있는지 없는지 확인하는 함수
    /// </summary>
    /// <param name="toTargetDirection">대상으로 향하는 방향 백터</param>
    /// <returns>시야각 안에 있으면 true, 없으면 false</returns>
    bool IsInSightAngle(Vector3 toTargetDirection)
    {
        bool result = false;
        return result;
    }

    /// <summary>
    /// 대상을 바라보는 시야가 다른 오브젝트에 의해 가려졌는지 아닌지 확인하는 함수
    /// </summary>
    /// <param name="toTargetDirection">대상으로 향하는 방향 백터</param>
    /// <returns></returns>
    bool IsSightBlocked(Vector3 toTargetDirection)
    {
        bool result = false;
        return result;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // 원거리 시야 범위는 녹색으로 표시(부채꼴로 그리기)
        // 근거리 시야 범위는 노란색으로 표시
        // 단 플레이어가 시야 범위 안에 들어오면 빨간색
        
    }
#endif

}
