using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconRotator : MonoBehaviour
{
    /// <summary>
    /// 아이콘 회전 속도
    /// </summary>
    public float rotateSpeed = 360.0f;

    /// <summary>
    /// 아이콘이 위아래로 움직이는 속도
    /// </summary>
    public float moveSpeed = 2.0f;

    /// <summary>
    /// 아이콘의 최소 높이
    /// </summary>
    public float minHeight = 0.5f;

    /// <summary>
    /// 아이콘의 최대 높이
    /// </summary>
    public float maxHeight = 1.5f;

    /// <summary>
    /// 시간 누적용 변수(삼각함수용)
    /// </summary>
    float timeElapsed = 0.0f;

    private void Start()
    {
        transform.Rotate(0, Random.Range(0.0f, 360.0f), 0); // 처음에 초기회전 랜덤으로 적용
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime * moveSpeed;
        Vector3 pos;
        pos.x = transform.parent.position.x;
        pos.y = minHeight + (0.5f * (1-Mathf.Cos(timeElapsed)) * (maxHeight - minHeight));  // min ~ max로 왕복
        pos.z = transform.parent.position.z;

        // (Cos() + 1) * 0.5 => 1 ~ 0
        // 1 - (cos() + 1) * 0.5 => 0 ~ 1
        // min + (1 - (cos() + 1) * 0.5) => min ~ (1+min)
        // min + (1 - (cos() + 1) * 0.5) * (max-min) => min ~ max
        // (1 - (cos() + 1) * 0.5) = 1 - 0.5*cos() - 0.5 = 0.5 - 0.5*cos() = 0.5 * (1-cos())

        // 최종 : min + (0.5 * (1-cos) ) * (max-min)

        transform.position = pos;   // 실제 이동

        transform.Rotate(0, Time.deltaTime * rotateSpeed, 0);   // 추가 회전
    }
}
