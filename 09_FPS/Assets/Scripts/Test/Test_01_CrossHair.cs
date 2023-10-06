using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_01_CrossHair : TestBase
{
    public AnimationCurve curve;
    public Crosshair crosshair;
    public float expendAmount = 30.0f;

    [Range(0f, 1f)]
    public float testValue = 0.0f;

    protected override void Test1(InputAction.CallbackContext context)
    {
        Debug.Log(curve.Evaluate(testValue));
    }

    protected override void TestClick(InputAction.CallbackContext context)
    {
        crosshair.Expend(expendAmount);
    }
}
