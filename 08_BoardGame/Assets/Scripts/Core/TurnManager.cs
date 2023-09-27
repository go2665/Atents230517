using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    /// <summary>
    /// 현재 턴 번호
    /// </summary>
    int turnNumber = 1;

    /// <summary>
    /// 한턴이 타임아웃 되는데 걸리는 시간
    /// </summary>
    public float turnDuration = 5.0f;
    public float TurnDuration => turnDuration;

    /// <summary>
    /// 타임 아웃이 될 때까지 남은 시간
    /// </summary>
    float turnRemainTime = 0.0f;

    /// <summary>
    /// 현재 턴 진행상황을 표시하는 enum
    /// </summary>
    enum TurnProcessState
    {
        None = 0,   // 둘 다 행동을 완료 안함
        OneDone,    // 한명만 행동을 완료함
        BothDone,   // 둘 다 행동을 완료함
    }

    /// <summary>
    /// 현재 턴 진행 상태
    /// </summary>
    TurnProcessState state = TurnProcessState.None;

    /// <summary>
    /// 턴이 진행중인지를 표시하는 변수
    /// </summary>
    bool isTurnPlay = true;

    /// <summary>
    /// 턴이 시작될 때 실행될 델리게이트(파라메터:턴 번호)
    /// </summary>
    public Action<int> onTurnStart;

    /// <summary>
    /// 턴이 종료될 때 실행될 델리게이트
    /// </summary>
    public Action onTurnEnd;

    /// <summary>
    /// 턴 종료 처리중인지 확인하는 변수
    /// </summary>
    bool isEndProcess = false;

    /// <summary>
    /// 씬이 로드되었을 때 실행될 함수
    /// </summary>
    protected override void OnInitialize()
    {
        turnNumber = 0;                     // 각종 데이터 초기화

        if(GameManager.Inst.IsTestMode)     // 테스트 모드일 때는 타임아웃이 안 일어나도록 설정
        {
            turnDuration = float.MaxValue;
        }
        turnRemainTime = turnDuration;

        state = TurnProcessState.None;
        isTurnPlay = true;

        OnTurnStart();
    }

    /// <summary>
    /// 턴이 시작될 때 실행되는 함수(턴이 끝나면 자동으로 실행)
    /// </summary>
    void OnTurnStart()
    {
        if(isTurnPlay)
        {
            turnNumber++;   // 턴 번호 증가

            Debug.Log($"{turnNumber}턴 시작");
            state = TurnProcessState.None;      // 턴 진행 상황 리셋
            turnRemainTime = turnDuration;      // 턴 진행 시간 리셋

            onTurnStart?.Invoke(turnNumber);    // 턴이 시작되었음을 알림
        }
    }

    /// <summary>
    /// 턴이 종료될 때 실행되는 함수
    /// </summary>
    void OnTurnEnd()
    {
        if (isTurnPlay)
        {
            isEndProcess = true;                // 턴 종료 처리가 시작되었다고 표시
            onTurnEnd?.Invoke();                // 턴이 종료되었음을 알림
            Debug.Log($"{turnNumber}턴 종료");

            isEndProcess = false;               // 턴 종료 처리가 끝났다고 표시
            OnTurnStart();                      // 다음 턴 시작
        }
    }

    private void Update()
    {
        turnRemainTime -= Time.deltaTime;           // 턴 진행시간 처리
        if( isTurnPlay && turnRemainTime < 0.0f )   // 턴 매니저가 진행되고 있고 남은 시간이 0 미만이면
        {
            OnTurnEnd();                            // 턴 종료
        }
    }

    /// <summary>
    /// 턴 진행 상황 확인용 함수(플레이어의 행동이 끝났을 때 실행)
    /// </summary>
    public void CheckTurnEnd()
    {
        if( !isEndProcess )     // 턴 종료 처리 중에는 체크하지 않음
        {
            state++;            // 다음 단계로 상태 변경
            if (state >= TurnProcessState.BothDone)
            {
                OnTurnEnd();    // 둘 다 행동이 끝나면 턴 종료
            }
        }        
    }

    /// <summary>
    /// 턴 진행을 멈추는 함수
    /// </summary>
    public void TurnStop()
    {
        isTurnPlay = false; // 더 이상 턴이 진행되지 않게 막기
    }

}
