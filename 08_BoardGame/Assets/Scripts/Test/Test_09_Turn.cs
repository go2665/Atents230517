using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_09_Turn : Test_08_PlayerAttack
{
    protected override void Start()
    {
        base.Start();

        user.AutoShipDeployment(true);
    }

    protected override void OnR_Click(InputAction.CallbackContext context)
    {
        enemy.AutoAttack();
    }
}

// 실습
// 1. 턴에 맞춰서 진행하기
// 2. 모든 플레이어는 한턴에 한번만 공격할 수 있다.
// 3. 적은 턴이 시작하면 1~2초(랜덤)를 기다렸다가 자동 공격을 한다.
// 4. 유저는 클릭을 해서 공격 할 수 있다.(이미 공격을 했으면 이후는 눌러도 소용 없다)