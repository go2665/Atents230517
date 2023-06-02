using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAsteroidMini : EnemyBase
{
    float baseSpeed = 7.0f;
    float rotateSpeed = 0.0f;
    Vector3 direction;

    public override void OnInitialize()
    {
        speed = baseSpeed + Random.Range(-1.0f, 1.0f);  // 이동 속도 랜덤
        rotateSpeed = Random.Range(0.0f, 360.0f);       // 회전 속도 랜덤
        direction = -transform.right;                   // 이동 방향 기록
    }

    protected override void OnMoveUpdate(float deltaTime)
    {
        transform.Translate(deltaTime * speed * direction, Space.World);    // 이동
        transform.Rotate(deltaTime * rotateSpeed * Vector3.forward);        // 회전
    }
}
