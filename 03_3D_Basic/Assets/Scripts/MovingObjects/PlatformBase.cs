using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBase : WaypointUser
{
    /// <summary>
    /// 어느 위치로 이동했다고 알리는 델리게이트
    /// </summary>
    public Action<Vector3> onMove;

    protected override void OnMove()
    {
        base.OnMove();
        onMove?.Invoke(moveDelta); // 실제로 움직이고 나서 신호보내기
    }
}
