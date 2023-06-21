using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRandomize : MonoBehaviour
{
    public bool check = true;

    // 인스펙터 창에서 데이터가 성공적으로 변화되면 실행되는 함수
    private void OnValidate()
    {
        if (check)
        {
            transform.Rotate(0, Random.Range(0.0f, 360.0f), 0); // 랜덤으로 회전
            transform.localScale = new Vector3(                 // 랜덤으로 스케일
                1 + Random.Range(-0.15f, 0.15f),
                1 + Random.Range(-0.15f, 0.15f),
                1 + Random.Range(-0.15f, 0.15f));
            check = false;
        }
    }
}
