using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombMark : MonoBehaviour
{
    public GameObject successPrefab;
    public GameObject failurePrefab;

    /// <summary>
    /// 공격 받은 위치에 포탄 명중 여부를 표시해주는 함수
    /// </summary>
    /// <param name="world">공격 받은 위치(월드좌표, 그리드의 가운데 위치)</param>
    /// <param name="isSuccess">공격이 성공했으면 true, 실패했으면 false</param>
    public void SetBombMark(Vector3 world, bool isSuccess)
    {
        // isSuccess에 따라 프리팹 생성
    }

    /// <summary>
    /// 모든 폭탄표시를 제거하는 함수
    /// </summary>
    public void ResetBombMarks()
    {

    }
}
