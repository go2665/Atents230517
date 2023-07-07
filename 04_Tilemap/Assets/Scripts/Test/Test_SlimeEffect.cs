using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_SlimeEffect : TestBase
{
    public Slime slime;

    void Start()
    {
        slime = FindObjectOfType<Slime>();
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        slime.TestShader(1);
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        slime.TestShader(2);
    }

    protected override void Test3(InputAction.CallbackContext context)
    {
        slime.TestShader(3);
    }

    protected override void Test4(InputAction.CallbackContext context)
    {
        slime.TestShader(4);
    }

    protected override void Test5(InputAction.CallbackContext context)
    {
        slime.TestShader(5);
    }
}
