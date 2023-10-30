using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BehaviourState : byte
{
    Wander,
    Chase,
    Attack,
    Dead
}

public enum HitLocation : byte
{
    Body,
    Head,
    Arm,
    Leg
}

public class Enemy : MonoBehaviour
{
    public float hp = 30.0f;
    public float HP
    {
        get => hp;
        set
        {
            hp = value;
            if(hp <= 0)
            {
                Die();
            }
        }
    }
    public float maxHp = 30.0f;

    public float walkSpeed = 2.0f;
    public float runSpeed = 7.0f;
    float speedPenalty = 0;

    BehaviourState state = BehaviourState.Dead;
    public BehaviourState State
    {
        get => state;
        set
        {
            if(state != value)
            {
                OnStateChange(value);
                state = value;
            }
        }
    }

    Action onUpdate = null;

    public Action<Enemy> onDie;

    public float sightAngle = 90.0f;
    public float sightRange = 20.0f;

    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        SphereCollider sphere = GetComponent<SphereCollider>();
        sphere.radius = sightRange;
        State = BehaviourState.Wander;
    }

    private void OnEnable()
    {
        HP = maxHp;
        agent.speed = walkSpeed;
        speedPenalty = 0;
    }

    private void Update()
    {
        onUpdate();
    }

    void Update_Wander()
    {
        if(PlayerFind())
        {
            State = BehaviourState.Chase;
        }
        else if (!agent.pathPending && agent.remainingDistance <= 0)
        {
            Vector3 destination = GetRandomDestination();
            agent.SetDestination(destination);
            Debug.Log($"Dest : {destination}");
        }
    }

    void Update_Chase()
    {
        // 마지막 목격한 장소까지 이동
        // 마지막 목격 장소에 도착했는데 플레이어가 없으면 다시 배회 상태로
    }

    void Update_Attack()
    {

    }

    void Update_Dead()
    {

    }

    Vector3 GetRandomDestination()
    {
        Vector3 result = new();
        float size = CellVisualizer.CellSize;

        Vector2Int current = new((int)(transform.position.x / size), (int)(-transform.position.z / size));
        Vector2Int target = new(
            UnityEngine.Random.Range(current.x - 3, current.x + 4), UnityEngine.Random.Range(current.y - 3, current.y + 4));

        result.x = (target.x + 0.5f) * size;
        result.y = 0.0f;
        result.z = -(target.y + 0.5f) * size;

        return result;
    }

    private void Die()
    {
        onDie?.Invoke(this);
        gameObject.SetActive(false);
    }

    public void OnAttacked(HitLocation hitLocation, float damage)
    {
        switch(hitLocation)
        {
            case HitLocation.Body:
                HP -= damage;
                //Debug.Log("몸통을 맞았다.");
                break;
            case HitLocation.Head:
                HP -= damage * 2;
                //Debug.Log("머리를 맞았다.");
                break;
            case HitLocation.Arm:
                HP -= damage;
                // 공격력 감소
                //Debug.Log("팔을 맞았다.");
                break;
            case HitLocation.Leg:
                speedPenalty += 1;
                agent.speed = walkSpeed - speedPenalty;
                //Debug.Log("다리을 맞았다.");
                break;
        }
    }

    private void OnStateChange(BehaviourState newState)
    {
        switch (newState)
        {
            case BehaviourState.Wander:
                onUpdate = Update_Wander;
                agent.speed = walkSpeed;
                break;
            case BehaviourState.Chase:
                onUpdate = Update_Chase;
                agent.speed = runSpeed;
                break;
            case BehaviourState.Attack:
                onUpdate = Update_Attack;
                break;
            case BehaviourState.Dead:
                onUpdate = Update_Dead;
                agent.speed = 0.0f;
                break;
        }
    }

    Transform target = null;
    Collider[] playerCollider = new Collider[1];
    bool PlayerFind()
    {
        bool result = false;

        if(target != null &&
            Physics.OverlapSphereNonAlloc(transform.position, sightRange, playerCollider, LayerMask.GetMask("Player")) > 0 )
        {
            // 벽에 가려지는가?
            Vector3 dir = playerCollider[0].transform.position - transform.position;
            Ray ray = new(transform.position, dir);
            if (Physics.Raycast(ray, out RaycastHit hit, sightRange, LayerMask.GetMask("Player")))
            {
                if (hit.collider == playerCollider[0])
                {
                    // 플레이어와의 사이에 가리는게 없다.

                    // 시야각 안에 들어있는가?
                    float angle = Vector3.Angle(transform.forward, dir);
                    if (angle * 2 < sightAngle)
                    {
                        // 플레이어가 적의 시야각 안에 있다.
                        result = true;
                        Debug.Log("플레이어 발견");
                    }
                }
            }
        }

        return result;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            target = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = null;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 p0 = transform.position + Vector3.up;
        Vector3 f = transform.forward * 5;

        Vector3 p1 = Quaternion.Euler(0, sightAngle * 0.5f, 0) * f + p0;
        Vector3 p2 = Quaternion.Euler(0, -sightAngle * 0.5f, 0) * f + p0;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(p0, p1);
        Gizmos.DrawLine(p0, p2);
    }
}


// 1. 적은 상태를 가진다.
//  1.1. 순찰 상태 : 랜덤으로 계속 이동
//  1.2. 추적 상태 : 플레이어가 시야에 들어오면 추적 상태가 되어서 마지막으로 목격한 장소까지 이동. (더 이상 발견이 안되면 순찰상태로 돌아감)
//  1.3. 공격 상태 : 추적 상태인데 공격 범위안에 들어오면 공격 시작
//  1.4. 사망 상태 : HP가 0 이하이면 진입. 일정 시간 후에 부활해서 다시 순찰 상태로