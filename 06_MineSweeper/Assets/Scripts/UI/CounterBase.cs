using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ImageNumber))]
public class CounterBase : MonoBehaviour
{
    ImageNumber imageNumber;

    protected virtual void Awake()
    {
        imageNumber = GetComponent<ImageNumber>();
    }

    /// <summary>
    /// 델리게이트에 연결할 함수
    /// </summary>
    /// <param name="count">새 숫자</param>
    protected void Refresh(int count)
    {
        imageNumber.Number = count;
    }

}
