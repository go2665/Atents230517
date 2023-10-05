using Cinemachine;
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
                Input.ResetBind();
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

    // 카메라 용 -----------------------------------------------------------------------------------
    CinemachineImpulseSource impulseSource;
    public CinemachineImpulseSource ImpulseSource => impulseSource;

    // 테스트용 ------------------------------------------------------------------------------------
    public bool IsTestMode = true;

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
        if(user != null )
        {
            onStateChange += user.OnStateChange;    // 순서가 답이 없음
        }
        if(enemy != null )
        {
            onStateChange += enemy.OnStateChange;
        }

        CinemachineVirtualCamera vcam = FindAnyObjectByType<CinemachineVirtualCamera>();
        if( vcam != null )
        {
            impulseSource = vcam.GetComponent<CinemachineImpulseSource>();
        }
    }

    /// <summary>
    /// 함선 배치 정보 저장용 함수
    /// </summary>
    /// <param name="targetPlayer">배치 정보를 저장할 플레이어</param>
    /// <returns>true면 저장 성공, false면 저장 실패(배치되어있지 않은 배가 있다.)</returns>
    public bool SaveShipDeployData(UserPlayer targetPlayer)
    {        
        bool result = false;
        if( targetPlayer.IsAllDeployed)    //모든 배가 배치되어있을 때만 저장
        {
            shipDeployDatas = new ShipDeployData[targetPlayer.Ships.Length];
            for(int i=0;i<shipDeployDatas.Length; i++)
            {
                Ship ship = targetPlayer.Ships[i];
                shipDeployDatas[i] = new ShipDeployData(ship.Direction, ship.Positions[0]); // 함선의 위치와 방향 저장
            }
            result = true;
        }

        return result;
    }

    /// <summary>
    /// 함선 배치 정보 로딩용 함수
    /// </summary>
    /// <param name="targetPlayer">저장된 데이터를 로딩할 플레이어</param>
    /// <returns>성공여부. true면 성공, false면 저장된 데이터가 없다.</returns>
    public bool LoadShipDeployData(UserPlayer targetPlayer)
    {
        bool result = false;
        if(shipDeployDatas != null)
        {
            targetPlayer.UndoAllShipDeployment();                   // 일단 모든 배치 취소
            for (int i=0; i<shipDeployDatas.Length; i++)
            {
                Ship ship = targetPlayer.Ships[i];
                ship.Direction = shipDeployDatas[i].Direction;      // 배 방향 설정
                targetPlayer.Board.ShipDeployment(ship, shipDeployDatas[i].Position);   // 배 배치
                ship.gameObject.SetActive(true);                    // 게임 오브젝트 보이게 만들기
            }
            result = true;
        }
        return result;
    }
}
