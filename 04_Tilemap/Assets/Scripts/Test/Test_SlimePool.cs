using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_SlimePool : TestBase
{
    List<GameObject> slimeList = new List<GameObject>(64);

    protected override void Test1(InputAction.CallbackContext context)
    {
        slimeList.Add(
            Factory.Inst.GetObject(
                PoolObjectType.Slime, 
                new Vector3(Random.Range(-8.0f,8.0f), Random.Range(-4.0f,4.0f)) 
            )
        );
    }

    protected override void Test2(InputAction.CallbackContext context)
    {
        while ( slimeList.Count > 0 )
        {
            GameObject obj = slimeList[0];
            slimeList.RemoveAt(0);
            obj.SetActive( false );
        }
    }
}

// 실습
// 1. 슬라임이 풀에서 나올 때 셰이더 프로퍼티가 초기화되고 페이즈가 실행된다.
// 2. 슬라임의 Die 함수 구현하기
