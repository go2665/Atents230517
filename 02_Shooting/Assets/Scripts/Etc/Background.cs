using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    /// <summary>
    /// 배경 한줄(슬롯)
    /// </summary>
    public Transform[] bgSlots;

    /// <summary>
    /// 배경 이동 속도
    /// </summary>
    public float scrollingSpeed = 2.5f;

    /// <summary>
    /// 배경 이미지 한변의 길이
    /// </summary>
    const float BackgroundWidth = 13.6f;

    protected virtual void Awake()
    {
        bgSlots = new Transform[transform.childCount];  // 자식 갯수만큼의 크기를 가지는 배열 만들기
        for(int i=0;i<bgSlots.Length; i++)
        {
            bgSlots[i] = transform.GetChild(i);         // 배열 안에 트랜스폼 넣기
        }
    }

    private void Update()
    {
        float baseLineX = transform.position.x - BackgroundWidth;   // 충분히 왼쪽으로 갔는지 확인할 기준선 계산

        for(int i=0;i<bgSlots.Length;i++)           // 모든 슬롯 움직이기
        {
            bgSlots[i].Translate(Time.deltaTime * scrollingSpeed * -transform.right);   // bgSlot들을 왼쪽으로 이동시키기
            if (bgSlots[i].position.x < baseLineX)  // 기준선을 넘었으면
            {
                MoveRightEnd(i);                    // 오른쪽 끝으로 보내기
            }
        }
    }

    /// <summary>
    /// 오른쪽 끝으로 보내는 함수
    /// </summary>
    /// <param name="index">오른쪽 끝으로 보낼 슬롯의 인덱스</param>
    protected virtual void MoveRightEnd(int index)
    {
        bgSlots[index].Translate(BackgroundWidth * bgSlots.Length * transform.right);
    }
}
