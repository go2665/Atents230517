using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_05_EndProcess : TestBase
{
    public Transform shellTransform;
    public PlayerBase player;

    protected override void Test1(InputAction.CallbackContext context)
    {
        //Factory.Inst.GetNormalShell(shellTransform);
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        player.DamageTaken(35, new(1,0,0));
    }

    protected override void Test3(InputAction.CallbackContext context)
    {
        Rigidbody rigid = player.GetComponent<Rigidbody>();
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        player.transform.position = new(3, 0, 0);
        player.transform.rotation = Quaternion.identity;
    }


}
