using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_06_BoardAttacked : Test_05_ShipDeploymentAuto
{
    protected override void TestClick(InputAction.CallbackContext context)
    {
        base.TestClick(context);

        // 실습
        // 1. 클릭을 했을 때 TargetShip이 없으면 보드에 공격
        // 2. Board.OnAttacked 함수 구현
        //   2.1. 공격당한 위치가 보드 안이 아니거나 공격이 있었던 위치는 아무런 처리 없음
        //   2.2. 공격당한 위치가 공격 당한적이 없으면 BombMark를 이용해서 결과 표시
    }
}
