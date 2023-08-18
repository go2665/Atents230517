using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // 게임 상태 관련 ------------------------------------------------------------------------------
    public enum GameState
    {
        Ready = 0,  // 게임 시작 전(시작하고 아무런 행동이 없는 상태)
        Play,       // 게임 진행 중(첫번째 셀이 열린 이후나 깃발이 설치된 이후)
        GameClear,  // 모든 지뢰를 찾았을 때
        GameOver    // 지뢰가 있는 셀을 열었을 때
    }

    /// <summary>
    /// 게임의 현재 상태
    /// </summary>
    GameState state = GameState.Ready;

    GameState State
    {
        get => state;
        set
        {
            if( state != value)
            {
                state = value;
                switch (state)
                {
                    case GameState.Ready:
                        onGameReady?.Invoke();
                        break;
                    case GameState.Play:
                        onGamePlay?.Invoke();
                        break;
                    case GameState.GameClear:
                        onGameClear?.Invoke();
                        break;
                    case GameState.GameOver:
                        onGameOver?.Invoke();
                        break;
                }
                Debug.Log($"현재 상태 : {state}");
            }
        }
    }

    /// <summary>
    /// 게임이 진행중인지 아닌지 확인하기 위한 프로퍼티
    /// </summary>
    public bool IsPlaying => State == GameState.Play;

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


    // 보드 관련 -----------------------------------------------------------------------------------
    private Board board;
    public Board Board => board;
    public int mineCount = 10;
    public int boardWitdth = 8;
    public int boardHeight = 8;
    // --------------------------------------------------------------------------------------------

    // UI 관련 ------------------------------------------------------------------------------------
    
    /// <summary>
    /// 플레이어의 행동 횟수
    /// </summary>
    private int actionCount = 0;

    /// <summary>
    /// 행동 횟수를 확인하고 설정하는 프로퍼티
    /// </summary>
    public int ActionCount
    {
        get => actionCount;
        private set
        {
            if( actionCount != value)   // 횟수가 변경되면
            {
                actionCount = value;
                onActionCountChange?.Invoke(actionCount);   // 델리게이트 실행
            }
        }
    }
    /// <summary>
    /// 행동 횟수가 변경되었을 때 실행될 델리게이트
    /// </summary>
    public Action<int> onActionCountChange;


    // --------------------------------------------------------------------------------------------


    /// <summary>
    /// 초기화용 함수
    /// </summary>
    protected override void OnInitialize()
    {
        FlagCount = mineCount;                                  // 깃발 개수 설정
        board = FindObjectOfType<Board>();                      // 보드 가져와서
        board.Initialize(boardWitdth, boardHeight, mineCount);  // 보드 생성하기
    }

    /// <summary>
    /// 깃발 개수를 하나 증가시키는 함수
    /// </summary>
    public void IncreaseFlagCount()
    {
        FlagCount++;
    }

    /// <summary>
    /// 깃발 개수를 하나 감소시키는 함수
    /// </summary>
    public void DecreaseFlagCount()
    {
        FlagCount--;
    }

    /// <summary>
    /// 플레이어 행동이 끝났을 때 실행될 함수
    /// </summary>
    public void FinishPlayerAction()
    {
        ActionCount++;          // 행동 회수 증가

        if(Board.IsBoardClear)  // 클리어된 상황인지 확인
        {
            GameClear();        // 클리어되었으면 클리어 처리
        }
    }

    public void GameStart()
    {
        if(State == GameState.Ready)
        {
            State = GameState.Play;
        }
    }

    public void GameReset()
    {
        // 게임 초기화하고 레디 상태로 변경하기
        FlagCount = mineCount;
        ActionCount = 0;
        State = GameState.Ready;
    }

    public void GameOver()
    {
        State = GameState.GameOver;
    }

    public void GameClear()
    {
        State = GameState.GameClear;
    }

    // 테스트 코드 -------------------------------------------------------------------------------------
    public void Test_Flag(int flag)
    {
        FlagCount = flag;
    }

    public void Test_State(GameState state)
    {
        switch(state)
        {
            case GameState.Ready:
                onGameReady?.Invoke();
                break;
            case GameState.Play:
                onGamePlay?.Invoke();
                break;
            case GameState.GameClear:
                onGameClear?.Invoke();
                break;
            case GameState.GameOver: 
                onGameOver?.Invoke();
                break;
        }
    }
}
