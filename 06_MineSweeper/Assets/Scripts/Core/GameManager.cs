using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // 게임 상태 관련 ------------------------------------------------------------------------------
    enum GameState
    {
        Ready = 0,  // 게임 시작 전(첫번째 셀이 아직 열리지 않은 상태)
        Play,       // 게임 진행 중(첫번째 셀이 열린 이후)
        GameClear,  // 모든 지뢰를 찾았을 때
        GameOver    // 지뢰가 있는 셀을 열었을 때
    }

    /// <summary>
    /// 게임의 현재 상태
    /// </summary>
    GameState state = GameState.Ready;

    /// <summary>
    /// 게임이 재시작되면 초기화 되었을 때 실행될 델리게이트
    /// </summary>
    public Action onGameReady;

    /// <summary>
    /// 게임이 Play 상태가 되었을 때 실행될 델리게이트
    /// </summary>
    public Action onGamePlay;

    /// <summary>
    /// 게임이 Clear 상태가 되었을 때 실행될 델리게이트
    /// </summary>
    public Action onGameClear;

    /// <summary>
    /// 게임이 Over 상태가 되었을 때 실행될 델리게이트
    /// </summary>
    public Action onGameOver;
    // --------------------------------------------------------------------------------------------



    // 깃발 개수 관련 ------------------------------------------------------------------------------
    
    /// <summary>
    /// 깃발 개수
    /// </summary>
    private int flagCount = 0;

    /// <summary>
    /// 깃발 개수 확인 및 설정용 프로퍼티
    /// </summary>
    public int FlagCount
    {
        get => flagCount;
        private set
        {
            flagCount = value;
            onFlagCountChange?.Invoke(flagCount);   // 깃발 개수가 변경되면 알람 보냄
        }
    }

    /// <summary>
    /// 깃발 개수가 변경될 때마다 실행되는 델리게이트
    /// </summary>
    public Action<int> onFlagCountChange;
    // --------------------------------------------------------------------------------------------
}
