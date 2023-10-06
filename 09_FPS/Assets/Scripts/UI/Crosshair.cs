using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    /// <summary>
    /// 회복 속도 조절을 위한 커브
    /// </summary>
    public AnimationCurve recoveryCurve;

    /// <summary>
    /// 최대 확장 크기
    /// </summary>
    public float maxExpend = 100.0f;

    /// <summary>
    /// 기본 확장 크기(최소)
    /// </summary>
    const float defaultExpend = 10.0f;

    /// <summary>
    /// 현재 확장 크기
    /// </summary>
    float current = 0.0f;

    /// <summary>
    /// 조준선들의 RectTransform
    /// </summary>
    RectTransform[] crossRects;

    /// <summary>
    /// 조준선들 이동 방향 계산용
    /// </summary>
    readonly Vector2[] direction = {Vector2.up, Vector2.right, Vector2.down, Vector2.left};

    private void Awake()
    {
        crossRects = new RectTransform[transform.childCount];   // 조준선들의 RectTransform 전부 찾기
        for(int i = 0; i < crossRects.Length; i++)
        {
            crossRects[i] = transform.GetChild(i) as RectTransform;
        }
    }

    /// <summary>
    /// 조준선을 사방으로 펼쳐지게 만드는 함수
    /// </summary>
    /// <param name="amount">펼쳐지는 정도</param>
    public void Expend(float amount)
    {
        current = Mathf.Min(current + amount, maxExpend);               // 확장은 최대치까지만
        for(int i = 0;i < crossRects.Length;i++)
        {
            crossRects[i].anchoredPosition = current * direction[i];    // current 만큼 확장
        }
        StopAllCoroutines();                    // 이전 코루틴은 정지
        StartCoroutine(DelayRecovery(0.1f));    // 디폴트 상태로 되돌리는 코루틴
    }

    /// <summary>
    /// 조준선을 디폴트 상태로 되돌리는 코루틴
    /// </summary>
    /// <param name="wait">처음 기다리는 시간</param>
    /// <returns></returns>
    IEnumerator DelayRecovery(float wait)
    {
        yield return new WaitForSeconds(wait);  // wait초 만큼 대기

        float startExpend = current;    // 현재 확장 정도를 최대치로 저장
        float curveProcess = 0.0f;      // 커브 진행 상황(0~1)
        float duration = 0.5f;          // 조준선이 디폴트 상태로 가는데 걸리는 시간
        float div = 1 / duration;       // 나누기 결과를 미리 저장해 놓기(나누기 회수를 최소화시키기 위한 용도)

        while(curveProcess < 1)         // duration초 동안 반복되게 하기
        {
            curveProcess += Time.deltaTime * div;
            current = recoveryCurve.Evaluate(curveProcess) * startExpend;   // 커브를 이용해 current 계산
            for(int i=0;i < crossRects.Length;i++)
            {
                crossRects[i].anchoredPosition = (current + defaultExpend) * direction[i];  // 계산 결과대로 점점 축소 시키기
            }
            yield return null;          // 1프레임 동안 대기
        }

        // 마지막으로 위치를 깔끔하게 지정
        for(int i=0;i< crossRects.Length;i++)
        {
            crossRects[i].anchoredPosition = defaultExpend * direction[i];
        }
        current = 0.0f;                 // current 클리어
    }
}
