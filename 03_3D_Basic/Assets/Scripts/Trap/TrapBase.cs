using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBase : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 무언가가 트리거 안에 들어오면 함정을 발동
        OnTrapActivate(other.gameObject);   
    }

    /// <summary>
    /// 함정이 발동될 때 실행되는 함수
    /// </summary>
    /// <param name="target">함정에 들어온 대상</param>
    protected virtual void OnTrapActivate(GameObject target) 
    { 
    }
}
