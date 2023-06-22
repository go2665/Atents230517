using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Turret : TestBase
{
    Transform fire;

    private void Start()
    {
        fire = transform.GetChild(0);
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        Factory.Inst.GetObject(PoolObjectType.Bullet, fire.position);
    }
}
