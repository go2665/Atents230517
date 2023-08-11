using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    /// <summary>
    /// 타이머가 측정을 시작한 이후의 경과 시간
    /// </summary>
    float elapsedTime = 0.0f;

    /// <summary>
    /// 경과시간 확인용 프로퍼티
    /// </summary>
    public float ElapsedTime => elapsedTime;

    /// <summary>
    /// 실제로 보이는 시간(elapsedTime의 자연수부분, 초가 변경되었음을 확인하기 위한 용도)
    /// </summary>
    int visibleTime = 0;

    /// <summary>
    /// 초 단위로 시간이 변경될 때 실행될 델리게이트
    /// </summary>
    public Action<int> onTimeChange;

    /// <summary>
    /// 시간 진행용 코루틴
    /// </summary>
    IEnumerator timeCoroutine;

    private void Start()
    {
        timeCoroutine = TimeProcess();

        GameManager manager = GameManager.Inst;
        manager.onGameReady += TimerReset;
        manager.onGamePlay += TimerReset;
        manager.onGamePlay += Play;
        manager.onGameClear += Stop;
        manager.onGameOver += Stop;
    }

    /// <summary>
    /// 타이머의 시간 측정을 시작하는 함수
    /// </summary>
    void Play()
    {
        StartCoroutine(timeCoroutine);
    }

    /// <summary>
    /// 타이머의 시간 측정을 정지하는 함수
    /// </summary>
    void Stop()
    {
        StopCoroutine(timeCoroutine);
    }

    /// <summary>
    /// 타이머를 초기화하고 정지하는 함수
    /// </summary>
    void TimerReset()
    {
        elapsedTime = 0.0f;
        onTimeChange?.Invoke(0);
        StopCoroutine(timeCoroutine);
    }

    /// <summary>
    /// 시간 진행시키는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator TimeProcess()
    {
        while(true)
        {
            elapsedTime += Time.deltaTime;
            if(visibleTime != (int)elapsedTime)
            {
                visibleTime = (int)elapsedTime;
                onTimeChange?.Invoke(visibleTime);
            }
            yield return null;
        }
    }
}
