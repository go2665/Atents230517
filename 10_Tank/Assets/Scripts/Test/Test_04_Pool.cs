using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class Test_04_Pool : TestBase
{
    public Transform fire;

    protected override void Test1(InputAction.CallbackContext context)
    {
        Factory.Inst.GetExplosion(Vector3.zero, Vector3.up);        
        
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        //Factory.Inst.GetNormalShell(fire);
    }

    protected override void Test3(InputAction.CallbackContext context)
    {        
    }
}
