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
        GameObject prefab = isSuccess ? successPrefab : failurePrefab;  // 프리팹 결정
        GameObject instance = Instantiate(prefab, transform);           // 프리팹 생성
        world.y = transform.position.y;
        instance.transform.position = world;                            // 위치 이동
    }

    /// <summary>
    /// 모든 폭탄표시를 제거하는 함수
    /// </summary>
    public void ResetBombMarks()
    {
        while(transform.childCount > 0)                 // 자식이 남아있으면 계속 반복
        {
            Transform child = transform.GetChild(0);    // 첫번째 자식 제거하기
            child.SetParent(null);
            Destroy(child.gameObject);
        }
    }
}
