using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_06_BoardAttacked : Test_05_ShipDeploymentAuto
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void TestClick(InputAction.CallbackContext context)
    {
        Vector2 screen = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(screen);

        if (TargetShip != null) 
        {
            if( board.ShipDeployment(TargetShip, world))   // 선택된 배가 있을 때 우선 배치 시도)
            {
                Debug.Log("함선배치 성공");
                TargetShip = null;          // 배 선택 해제
            }
            else
            {
                Debug.Log("함선배치 실패");
            }
        }
        else
        {
            board.OnAttacked(board.WorldToGrid(world));
        }

        // 실습
        // 1. 클릭을 했을 때 TargetShip이 없으면 보드에 공격
        // 2. BombMark 클래스 구현하기
        // 3. Board.OnAttacked 함수 구현
        //   3.1. 공격당한 위치가 보드 안이 아니거나 공격이 있었던 위치는 아무런 처리 없음
        //   3.2. 공격당한 위치가 공격 당한적이 없으면 BombMark를 이용해서 결과 표시
    }
}
