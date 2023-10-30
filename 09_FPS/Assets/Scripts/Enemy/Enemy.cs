using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.LowLevel;

public enum BehaviourState : byte
{
    Wander,
    Chase,
    Find,
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
                OnStateExit(state);
                state = value;
                OnStateEnter(state);
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
            //Debug.Log($"Dest : {destination}");
        }
    }

    void Update_Chase()
    {
        // 마지막 목격한 장소까지 이동
        if( IsPlayerInSight(out Vector3 newPostion) )
        {
            agent.SetDestination(newPostion);
            Debug.Log($"목적지 갱신 : {newPostion}");
        }
        else if (!agent.pathPending && agent.remainingDistance <= 0)
        {
            // 플레이어가 안보이는데 마지막으로 목격한 장소에 도착했다. => 다시 배회 상태로
            Debug.Log($"배회 상태로 전환");
            State = BehaviourState.Find;
        }
    }

    public float findTime = 5.0f;
    float findTimeElapsed = 5.0f;
    void Update_Find()
    {
        findTimeElapsed -= Time.deltaTime;
        if(findTimeElapsed < 0 )
        {
            State = BehaviourState.Wander;  // 일정 시간까지 플레이어 못찾았다. => 배회
        }

        if (PlayerFind())
        {
            State = BehaviourState.Chase;   // 플레이어를 찾았다 => 추적
        }   
    }

    public float attackPower = 10.0f;
    void Update_Attack()
    {
        // 적
        // 1. chase 상태에서 일정거리 안으로 플레이어가 들어오면 공격 상태로 변경된다.
        // 2. 공격 상태일 때는 플레이어를 무조건 계속 쫒아온다.
        // 3. 플레이어가 죽었다 => 배회 상태
        // 4. 적이 죽었다. => 죽음 상태

        // 플레이어
        // 1. hp와 hpMax 만들기
        // 2. 피격용 함수 만들기
        //  2.1. hp 감소 => hp가 0이하면 플레이어 사망(디버그로 출력만)
        //  2.2. 몇시 방향에서 피격 당했는지 UI로 표시
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

    private void OnStateEnter(BehaviourState newState)
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
            case BehaviourState.Find:
                findTimeElapsed = findTime;
                onUpdate = Update_Find;
                agent.speed = walkSpeed;
                agent.angularSpeed = 360.0f;
                StartCoroutine(LookAround());
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

    private void OnStateExit(BehaviourState prevState)
    {
        switch (prevState)
        {
            case BehaviourState.Find:
                agent.angularSpeed = 120.0f;
                StopAllCoroutines();
                break;
            case BehaviourState.Wander:                
            case BehaviourState.Chase:
            case BehaviourState.Attack:
            case BehaviourState.Dead:
            default:
                break;
        }
    }

    Transform target = null;
    Collider[] playerCollider = new Collider[1];
    bool PlayerFind()
    {
        bool result = false;

        if(target != null)             
        {
            result = IsPlayerInSight(out _);
        }

        return result;
    }

    bool IsPlayerInSight(out Vector3 position)
    {
        bool result = false;
        position = Vector3.zero;
        if ( Physics.OverlapSphereNonAlloc(transform.position, sightRange, playerCollider, LayerMask.GetMask("Player")) > 0 )
        {
            // 벽에 가려지는가?
            Vector3 dir = playerCollider[0].transform.position - transform.position;
            Ray ray = new(transform.position + Vector3.up, dir);
            if (Physics.Raycast(ray, out RaycastHit hit, sightRange))
            {
                if (hit.collider == playerCollider[0])
                {
                    // 플레이어와의 사이에 가리는게 없다.

                    // 시야각 안에 들어있는가?
                    float angle = Vector3.Angle(transform.forward, dir);
                    if (angle * 2 < sightAngle)
                    {
                        // 플레이어가 적의 시야각 안에 있다.
                        position = playerCollider[0].transform.position;
                        result = true;
                        //Debug.Log("플레이어 발견");
                    }
                }
            }
        }

        return result;
    }

    IEnumerator LookAround()
    {
        // 두리번 거리기
        Vector3[] positions = { 
            transform.position - transform.right * 0.1f, 
            transform.position + transform.right * 0.1f,
            transform.position - transform.forward * 0.1f
        };
        int index = 0;
        int length = positions.Length;
        while(true)
        {
            agent.SetDestination(positions[index]);
            index = (index + 1) % length;
            yield return new WaitForSeconds(1);
        }
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
        Vector3 f = transform.forward * sightRange;

        Vector3 p1 = Quaternion.Euler(0, sightAngle * 0.5f, 0) * f + p0;
        Vector3 p2 = Quaternion.Euler(0, -sightAngle * 0.5f, 0) * f + p0;

        switch(State)
        {
            case BehaviourState.Wander:
                Gizmos.color = Color.green;
                break;
            case BehaviourState.Chase:
                Gizmos.color = Color.yellow;
                break;
            case BehaviourState.Find:
                Gizmos.color = Color.blue;
                break;
            case BehaviourState.Attack:
                Gizmos.color = Color.red;
                break;
            case BehaviourState.Dead:
                Gizmos.color = Color.black;
                break;
        }

        Gizmos.DrawLine(p0, p0+f);
        Gizmos.DrawLine(p0, p1);
        Gizmos.DrawLine(p0, p2);
    }
}


// 1. 적은 상태를 가진다.
//  1.1. 순찰 상태 : 랜덤으로 계속 이동
//  1.2. 추적 상태 : 플레이어가 시야에 들어오면 추적 상태가 되어서 마지막으로 목격한 장소까지 이동. (더 이상 발견이 안되면 순찰상태로 돌아감)
//  1.3. 공격 상태 : 추적 상태인데 공격 범위안에 들어오면 공격 시작
//  1.4. 사망 상태 : HP가 0 이하이면 진입. 일정 시간 후에 부활해서 다시 순찰 상태로