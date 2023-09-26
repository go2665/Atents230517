using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState : byte
{
    Title = 0,          // 타이틀 상태
    ShipDeployment,     // 함선 배치 상태
    Battle,             // 전투 상태
    GameEnd             // 게임 종료 상태
}

[RequireComponent(typeof(InputController))]
public class GameManager : Singleton<GameManager>
{
    // 플레이어 ------------------------------------------------------------------------------------
    UserPlayer user;
    public UserPlayer UserPlayer => user;

    EnemyPlayer enemy;
    public EnemyPlayer EnemyPlayer => enemy;

    // 게임 상태 -----------------------------------------------------------------------------------
    GameState gameState = GameState.Title;
    public GameState GameState
    {
        get => gameState;
        set
        {
            if( gameState != value )
            {
                gameState = value;
                onStateChange?.Invoke( gameState );
            }
        }
    }
    public Action<GameState> onStateChange;

    // 입력 ---------------------------------------------------------------------------------------
    InputController input;
    public InputController Input => input;

    // 함선 배치 정보 저장 -------------------------------------------------------------------------
    ShipDeployData[] shipDeployDatas;

    // 함수들 --------------------------------------------------------------------------------------
    protected override void OnPreInitialize()
    {
        base.OnPreInitialize();
        input = GetComponent<InputController>();
    }

    protected override void OnInitialize()
    {
        user = FindAnyObjectByType<UserPlayer>();
        enemy = FindAnyObjectByType<EnemyPlayer>();

        onStateChange = null;
    }

    /// <summary>
    /// 함선 배치 정보 저장용 함수
    /// </summary>
    /// <param name="targetPlayer">배치 정보를 저장할 플레이어</param>
    /// <returns>true면 저장 성공, false면 저장 실패(배치되어있지 않은 배가 있다.)</returns>
    public bool SaveShipDeployData(PlayerBase targetPlayer)
    {
        //모든 배가 배치되어있을 때만 저장
        bool result = true;
        shipDeployDatas = new ShipDeployData[targetPlayer.Ships.Length];
        for(int i=0;i<shipDeployDatas.Length; i++)
        {
            Ship ship = targetPlayer.Ships[i];
            shipDeployDatas[i] = new ShipDeployData(ship.Direction, ship.Positions[0]);
        }

        return result;
    }

    /// <summary>
    /// 함선 배치 정보 로딩용 함수
    /// </summary>
    /// <param name="targetPlayer">저장된 데이터를 로딩할 플레이어</param>
    /// <returns>성공여부. true면 성공, false면 저장된 데이터가 없다.</returns>
    public bool LoadShipDeployData( PlayerBase targetPlayer)
    {
        bool result = false;
        return result;
    }
}
