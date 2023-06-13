using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCurve : EnemyBase
{
    [Header("회전하는 적 데이터")]
    public float rotateSpeed = 10.0f;
    float curveDir = 0.0f;
    float? startY = null;

    public float StartY
    {
        set
        {
            if(startY == null)      // startY가 null 일 때만 방향 지정(enable 될 때마다 한번만 지정 가능)
            {
                startY = value;
                if (startY > 0)     // 시작 높이에 따라 커브 방향 지정
                {
                    // 위에서 등장하면 아래로 커브를 그리는 움직임을 한다.(우회전시켜야 한다)
                    curveDir = 1.0f;
                }
                else
                {
                    // 아래에서 등장하면 위로 커브를 그리는 움직임을 한다.(좌회전시켜야 한다)
                    curveDir = -1.0f;
                }
            }
        }
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        startY = null;          // 활성화 될 때마다 null로 초기화
    }

    protected override void OnMoveUpdate(float deltaTime)
    {
        base.OnMoveUpdate(deltaTime);
        transform.Rotate(deltaTime * rotateSpeed * curveDir * Vector3.forward);
    }
}

