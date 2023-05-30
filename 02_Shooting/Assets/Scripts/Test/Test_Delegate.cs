using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Test_Delegate : TestBase
{
    // 새로 만드는 델리게이트 타입의 이름은 TestDelegate
    // 이 델리게이트에서 저장할 수 있는 함수는, 파라메터가 없고 리턴타입이 void인 함수만 저장할 수 있다.
    public delegate void TestDelegate();

    // TestDelegate 타입으로 aaa라는 델리게이트를 만들었다.
    TestDelegate aaa;

    // 파라메터는 int하나와 float 하나를 받고 리턴타입은 void인 함수를 저장할 수 있는 델리게이트 타입 만들어보기
    public delegate void TestDelegate2(int a, float b);

    TestDelegate2 bbb;

    // Action : C#이 만들어 놓은 리턴이 없는(void) 함수를 저장할 수 있는 델리게이트
    Action ccc;         // 파라메터 없고 리턴타입 void인 델리게이트(C#이 미리 만들어 놓은 것)
    Action<int> ddd;    // 파라메터가 int 하나고 리턴타입이 void인 델리게이트

    // Func : C#이 만들어 놓은 리턴이 있는 함수를 저장할 수 있는 델리게이트(리턴타입은 마지막 제네릭)
    Func<int> eee;          // 파라메터가 없고 리턴타입이 int인 델리게이트
    Func<int, float> fff;   // 파라메터가 int 하나고 리턴타입이 float인 델리게이트

    // UnityEvent ggg;      // UnityEvent : Unity에서 제공하는 함수 저장하는 변수 타입

    private void Start()
    {
        aaa += DelTest1;
        bbb += DelTest2;    // DelTest2 함수 추가
        bbb += DelTest3;    // 이어서 DelTest3 추가
        bbb += DelTest2;    // 이어서 DelTest2 추가
        bbb -= DelTest3;    // DelTest3 하나 삭제
        bbb = DelTest3;     // 이전에 들어있던 것은 모두 무시하고 지금 것만 넣는다.
        bbb -= DelTest2;    // 빼는 것이 없으면 아무것도 안한다.

        //ggg.AddListener(DelTest1);

        //aaa += () => DelTest2(10, 5.0f);    // 이 람다식은 DelTest3과 같은 형식의 함수다.

        TestDelegate ccc = () => DelTest2(10, 5.0f);
        aaa += ccc;
        aaa -= ccc;
    }

    void DelTest3()
    {
        DelTest2(10, 5.0f);
    }

    void DelTest1()
    {
        Debug.Log("DelTest1");
    }

    void DelTest2(int i, float f)
    {
        Debug.Log($"DelTest2 - {i}, {f}");
    }

    void DelTest3(int i, float f)
    {
        Debug.Log($"DelTest3 - {i*2}, {f*2}");
    }

    protected override void Test1(InputAction.CallbackContext context)
    {
        aaa?.Invoke();
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        bbb?.Invoke(1, 2);
    }

}
