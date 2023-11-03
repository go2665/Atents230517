using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBase : MonoBehaviour
{
    static public int seed = -1;

    protected PlayerInputActions inputActions;

    private void Awake()
    {
        if( seed != -1 )
        {
            Random.InitState(seed);
        }
        inputActions = new PlayerInputActions();
    }

    protected virtual void OnEnable()
    {
        inputActions.Test.Enable();
        inputActions.Test.Test1.performed += Test1;
        inputActions.Test.Test2.performed += Test2;
        inputActions.Test.Test3.performed += Test3;
        inputActions.Test.Test4.performed += Test4;
        inputActions.Test.Test5.performed += Test5;
        inputActions.Test.TestClick.performed += TestClick;
    }

    protected virtual void OnDisable()
    {
        inputActions.Test.TestClick.performed -= TestClick;
        inputActions.Test.Test5.performed -= Test5;
        inputActions.Test.Test4.performed -= Test4;
        inputActions.Test.Test3.performed -= Test3;
        inputActions.Test.Test2.performed -= Test2;
        inputActions.Test.Test1.performed -= Test1;
        inputActions.Test.Disable();
    }

    protected virtual void Test1(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
    }

    protected virtual void Test2(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
    }

    protected virtual void Test3(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
    }

    protected virtual void Test4(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
    }

    protected virtual void Test5(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
    }

    protected virtual void TestClick(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
    }
}
