using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Waypoints 클래스를 사용하는 클래스
// 웨이포인트를 따라 이동함
public class WaypointUser : MonoBehaviour
{
    /// <summary>
    /// 이 오브젝트가 따라 움직일 웨이포인트들의 관리 클래스(필수)
    /// </summary>
    public Waypoints targetWaypoints;

    /// <summary>
    /// 이동 속도
    /// </summary>
    public float moveSpeed = 5.0f;

    /// <summary>
    /// 지금 현재 목표로하고 있는 웨이포인트의 트랜스폼
    /// </summary>
    Transform target;

    /// <summary>
    /// 이 오브젝트의 이동방향(target으로 가는 방향)
    /// </summary>
    Vector3 moveDir;

    /// <summary>
    /// 이번 물리 프레임에 이동한 정도
    /// </summary>
    protected Vector3 moveDelta = Vector3.zero;

    /// <summary>
    /// 목적지를 지정하는 프로퍼티
    /// </summary>
    protected virtual Transform Target
    {
        get => target;
        set
        {
            target = value;
            moveDir = (target.position - transform.position).normalized;    // 이동 방향 설정
        }
    }

    /// <summary>
    /// 현재 위치가 도착지점에 근접했는지 확인해주는 프로퍼티(true면 도착, false 도착안함)
    /// </summary>
    bool IsArrived
    {
        get
        {
            return (target.position - transform.position).sqrMagnitude < 0.02f;
        }
    }

    private void Start()
    {
        Target = targetWaypoints.CurrentWaypoint;
    }

    private void FixedUpdate()
    {
        OnMove();
    }

    /// <summary>
    /// 목적지에 도착했을 때 실행될 함수
    /// </summary>
    protected virtual void OnArrived()
    {
        Target = targetWaypoints.GetNextWaypoint();
    }

    /// <summary>
    /// 이동 처리용 함수. FixedUpdate에서 호출
    /// </summary>
    protected virtual void OnMove() 
    {
        //Debug.Log((Time.fixedDeltaTime * moveSpeed * moveDir).sqrMagnitude);
        moveDelta = Time.fixedDeltaTime * moveSpeed * moveDir;  // 이번 물리프레임에 움직인 정도를 저장
        transform.Translate(moveDelta, Space.World);
        
        if(IsArrived)
        {
            OnArrived();
        }
    }
}
