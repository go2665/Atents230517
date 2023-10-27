using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // 1. 플레이어의 총에 맞으면 죽는다.

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

    public float walkSpeed = 5.0f;
    public float runSpeed = 10.0f;

    public Action<Enemy> onDie;

    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        HP = maxHp;
    }

    private void Update()
    {
        if(!agent.pathPending && agent.remainingDistance <= 0)
        {
            Vector3 destination = GetRandomDestination();
            agent.SetDestination(destination);
            Debug.Log($"Dest : {destination}");
        }
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

    // 실습
    // 1. 이동 속도를 가진다.
    // 2. NavMeshAgent를 이용해 이동을 한다.
    //  2.1. 자신의 위치에서 +-3칸 안쪽을 랜덤으로 목적으로 설정한다.
    //  2.2. 목적지에 도착하면 다시 2.1.반복
}
