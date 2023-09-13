using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_03_ShipMake : TestBase
{
    public ShipType shipType = ShipType.Carrier;

    protected override void TestClick(InputAction.CallbackContext context)
    {
        // 실습
        // 마우스 클릭한 위치에 함선 생성하기 - ShipManager.Inst.MakeShip(shipType, null); 함수 사용

        Ship ship = ShipManager.Inst.MakeShip(shipType, null);

        Vector2 screen = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(screen);
        world.y = 0.0f;

        ship.transform.position = world;
    }
}
