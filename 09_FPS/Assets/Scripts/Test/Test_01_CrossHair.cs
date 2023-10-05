using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_01_CrossHair : TestBase
{
    public AnimationCurve curve;

    [Range(0f, 1f)]
    public float testValue = 0.0f;

    protected override void Test1(InputAction.CallbackContext context)
    {
        Debug.Log(curve.Evaluate(testValue));
    }
}
