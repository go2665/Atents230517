using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    /// <summary>
    /// 각 웨이포인트 지점의 트랜스폼
    /// </summary>
    Transform[] children;

    /// <summary>
    /// 현재 찾아갈 웨이포인트 지점의 인덱스
    /// </summary>
    int index = 0;

    /// <summary>
    /// 현재 웨이포인트를 확인하기 위한 프로퍼티
    /// </summary>
    public Transform Current => children[index];

    private void Awake()
    {
        children = new Transform[transform.childCount];
        for(int i=0; i<children.Length; i++)
        {
            children[i] = transform.GetChild(i);    // 자식 트랜스폼 찾아서 기록해 두기
        }
    }

    /// <summary>
    /// 다음 웨이포인트 지점을 리턴하는 함수
    /// </summary>
    /// <returns>다음 목적지</returns>
    public Transform MoveNext()
    {
        index++; 
        index %= children.Length;
        return children[index];
    }
}
