using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatfirnSwitch : PlatformBase, IInteractable
{
    public bool IsDirectUse => true;

    bool isMoveStart = false;

    private void Start()
    {
        Target = targetWaypoints.GetNextWaypoint(); // 첫번째 웨이포인트에 도착해서 안움직이는 현상 방지
    }

    private void FixedUpdate()
    {
        if (isMoveStart)
        {
            OnMove();
        }
    }

    protected override void OnArrived()
    {
        base.OnArrived();
        isMoveStart = false;    // 도착하면 멈추기
    }

    public void Use()
    {
        isMoveStart = true;     // 아이템 사용하면 움직이기
    }
}
