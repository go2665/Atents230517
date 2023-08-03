using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    EnemyState state = EnemyState.Patrol;

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
                        WaitTimer = waitTime;
                        onStateUpdate = Update_Wait;
                        break;
                    case EnemyState.Patrol:
                        agent.SetDestination(moveTarget.position);
                        onStateUpdate = Update_Patrol;
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
    /// 대기 시간
    /// </summary>
    public float waitTime = 1.0f;

    /// <summary>
    /// 대기 시간 측정용
    /// </summary>
    float waitTimer = 1.0f;
    protected float WaitTimer
    {
        get => waitTimer;
        set
        {
            waitTimer = value;
            if( waitTimer < 0.0f )
            {
                State = EnemyState.Patrol;
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
    public float sightRange;

    /// <summary>
    /// 시야각의 절반
    /// </summary>
    public float sightHalfAngle;

    /// <summary>
    /// 근접 시야 범위
    /// </summary>
    public float closeSightRange;

    /// <summary>
    /// 추적대상의 트랜스폼
    /// </summary>
    Transform chaseTarget;

    Animator animator;
    NavMeshAgent agent;
    SphereCollider bodyCollider;
    Rigidbody rigid;

    Action onStateUpdate;

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

    void Update_Wait()
    {
        WaitTimer -= Time.deltaTime;
    }

    void Update_Patrol()
    {
        if( agent.remainingDistance <= agent.stoppingDistance )
        {
            moveTarget = waypoints.MoveNext();
            State = EnemyState.Wait;
        }
    }

    void Update_Chase()
    {
    }

    void Update_Attack()
    {
    }

    void Update_Dead()
    {
    }


}
