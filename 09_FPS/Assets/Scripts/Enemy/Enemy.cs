using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    public float walkSpeed = 5.0f;
    public float runSpeed = 10.0f;
    float speedPenalty = 0;

    public Action<Enemy> onDie;

    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        HP = maxHp;
        agent.speed = walkSpeed;
        speedPenalty = 0;
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
}

public enum HitLocation
{
    Body,
    Head,
    Arm,
    Leg
}
