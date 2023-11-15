using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player2Tank : PlayerBase
{

    // 2. Player2Tank 만들기
    //  2.1. 이동은 WASD로 진행
    //  2.2. 발사는 Space로 진행
    //  2.3. 포탑은 움직일 수가 없다.
    // 3. 둘 중 하나의 탱크는 발사할 때 포탄이 랜덤으로 나간다.

    private void OnEnable()
    {
        inputActions.Player2.Enable();
        inputActions.Player2.Move.performed += OnMove;
        inputActions.Player2.Move.canceled += OnMove;
        inputActions.Player2.Fire.performed += OnChargeStart;
        inputActions.Player2.Fire.canceled += OnFire;
    }

    private void OnDisable()
    {
        inputActions.Player2.Fire.canceled -= OnFire;
        inputActions.Player2.Fire.performed -= OnChargeStart;
        inputActions.Player2.Move.canceled -= OnMove;
        inputActions.Player2.Move.performed -= OnMove;
        inputActions.Player2.Disable();
    }
}
