using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCurve : EnemyBase
{
    [Header("회전하는 적 데이터")]
    public float rotateSpeed = 10.0f;
    float curveDir = -1;

    public override void OnInitialize()
    {
        if( transform.position.y > 0)
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

    protected override void OnMoveUpdate(float deltaTime)
    {
        base.OnMoveUpdate(deltaTime);
        transform.Rotate(deltaTime * rotateSpeed * curveDir * Vector3.forward);
    }
}

