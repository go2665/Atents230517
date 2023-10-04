using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_13_CameraShake : TestBase
{
    public CinemachineImpulseSource source;

    protected override void Test1(InputAction.CallbackContext context)
    {
        source.GenerateImpulse();
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        source.GenerateImpulseAtPositionWithVelocity(transform.position, Vector3.right);
    }

    protected override void Test3(InputAction.CallbackContext context)
    {
        source.GenerateImpulseWithForce(10);
    }

    protected override void Test4(InputAction.CallbackContext context)
    {
        source.GenerateImpulseWithVelocity(Vector3.right + Vector3.up);
    }
}

// 실습
// 1. 배가 공격을 당하면 카메라가 랜덤한 방향으로 흔들린다.
// 2. 배가 침몰하면 더 크게 흔들린다.
//  참고) CinemachineImpulseSource는 게임 매니저에서 관리