using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RequireComponent: 이 스크립트를 사용하는데 필수적으로 필요한 컴포넌트를 표시하는 것
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
