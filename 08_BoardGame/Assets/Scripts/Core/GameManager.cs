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
    UserPlayer user;
    public UserPlayer UserPlayer => user;

    EnemyPlayer enemy;
    public EnemyPlayer EnemyPlayer => enemy;

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

    InputController input;
    public InputController Input => input;

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
}
