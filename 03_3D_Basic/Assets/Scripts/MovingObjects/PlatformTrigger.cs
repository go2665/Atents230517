using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어가 올라가면 다음 지점까지만 움직이는 플랫폼
public class PlatformTrigger : PlatformBase
{
    bool isMoveStart = false;

    private void Start()
    {
        Target = targetWaypoints.GetNextWaypoint(); // 첫번째 웨이포인트에 도착해서 안움직이는 현상 방지
    }

    private void FixedUpdate()
    {
        if(isMoveStart) 
        {
            OnMove();
        }
    }

    private void OnTriggerEnter(Collider other)
    {        
        isMoveStart = other.CompareTag("Player");   // 플레이어가 트리거 영역에 들어왔을 때 움직이기 시작
    }

    protected override void OnArrived()
    {
        base.OnArrived();
        isMoveStart = false;    // 도착하면 멈추기
    }
}
