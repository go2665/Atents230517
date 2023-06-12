using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAsteroidMini : EnemyBase
{
    float baseSpeed = 7.0f;
    float rotateSpeed = 0.0f;
    Vector3 direction = Vector3.zero;

    /// <summary>
    /// 방향 지정하는 프로퍼티(생성한 직후에 지정해야 함)
    /// </summary>
    public Vector3 Direction
    {
        get => direction;
        set
        {
            if (direction == Vector3.zero)  // 활성화 된 직후에 한번만 설정하기
            {
                direction = value;
            }
        }
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        speed = baseSpeed + Random.Range(-1.0f, 1.0f);  // 이동 속도 랜덤
        rotateSpeed = Random.Range(0.0f, 360.0f);       // 회전 속도 랜덤
        direction = Vector3.zero;                       // 활성화되면 방향은 (0,0,0)
    }

    protected override void OnMoveUpdate(float deltaTime)
    {
        transform.Translate(deltaTime * speed * direction, Space.World);    // 이동
        transform.Rotate(deltaTime * rotateSpeed * Vector3.forward);        // 회전
    }
}
