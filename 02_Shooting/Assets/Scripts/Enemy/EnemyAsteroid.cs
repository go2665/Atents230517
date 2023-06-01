using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAsteroid : EnemyBase
{
    /// <summary>
    /// 최소 이동 속도
    /// </summary>
    public float minMoveSpeed = 2.0f;
    /// <summary>
    /// 최대 이동 속도
    /// </summary>
    public float maxMoveSpeed = 4.0f;

    /// <summary>
    /// 최소 회전 속도
    /// </summary>
    public float minRotateSpeed = 30.0f;

    /// <summary>
    /// 최대 회전 속도
    /// </summary>
    public float maxRotateSpeed = 360.0f;

    /// <summary>
    /// 현재 회전 속도
    /// </summary>
    float rotateSpeed = 360.0f;

    /// <summary>
    /// 이동 방향
    /// </summary>
    private Vector3 direction;

    /// <summary>
    /// 목적지
    /// </summary>
    private Vector3? destination = null;

    /// <summary>
    /// 목적지 확인 및 설정용 프로퍼티
    /// </summary>
    public Vector3? Destination
    {
        get => destination;
        set
        {
            if (destination == null)    // destination은 null일 때만 세팅된다. (한번만 설정 가능하다)
            {
                destination = value;
                direction = (destination.Value - transform.position).normalized;    // 벡터의 크기를 1로 만들기(방향만 남겨 놓기)
                Debug.Log($"목적지 : {destination}");
            }
        }
    }

    /// <summary>
    /// 업데이트에서 실행되는 이동 처리 함수
    /// </summary>
    /// <param name="deltaTime">프레임간 경과시간</param>
    protected override void OnMoveUpdate(float deltaTime)
    {
        if (destination != null)
        {
            transform.Translate(deltaTime * speed * direction, Space.World);    // 이동(월드 기준)
        }
        transform.Rotate(deltaTime * rotateSpeed * Vector3.forward);            // 회전
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + direction);    // 운석 이동 방향 그리기
    }

    /// <summary>
    /// 클래스별 초기화 함수
    /// </summary>
    public override void OnInitialize()
    {
        base.OnInitialize();

        speed = Random.Range(minMoveSpeed, maxMoveSpeed);           // 속도만 랜덤으로 처리
        rotateSpeed = Random.Range(minRotateSpeed, maxRotateSpeed);
    }
}
