using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_03_Log : TestBase
{
    public Logger log;
    int i = 0;


    private void Start()
    {
        log.Clear();
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        log.Log(i.ToString());
        i++;
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        Debug.Log(log.TestIsPair("{aaaa}", '{', '}'));
        Debug.Log(log.TestIsPair("{aaaa", '{', '}'));
        Debug.Log(log.TestIsPair("aaaa}", '{', '}'));
        Debug.Log(log.TestIsPair("{{aaaa}", '{', '}'));
        Debug.Log(log.TestIsPair("{aaa}a}", '{', '}'));
        Debug.Log(log.TestIsPair("aaaa", '{', '}'));
    }

    protected override void Test3(InputAction.CallbackContext context)
    {
        log.Log("aaa[bbb]ccc{ddd}eee");
        log.Log("aaa[bbbccc{ddd}eee");
        log.Log("aaa[bbb]cccddd}eee");
        log.Log("aaa[bbbcccddd}eee");
        log.Log("aaa[bbb[ccc]ddd]eee");
    }
}


