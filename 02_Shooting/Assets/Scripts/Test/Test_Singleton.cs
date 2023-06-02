using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Singleton : TestBase
{
    protected override void Test1(InputAction.CallbackContext context)
    {
        //Singleton_Example.Instance.testI = 10;
        //Debug.Log(Singleton_Example.Instance.testI);
        Test_SingletonComponent.Inst.i = 30;
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        //Singleton_Example.Instance.testI += 15;
    }

    protected override void Test3(InputAction.CallbackContext context)
    {
        //Instantiate()
    }
}
